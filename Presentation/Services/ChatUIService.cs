using Edamame.Domain.Interfaces;

namespace Edamame.Presentation.Services
{
    /// chat UI logic 
    public class ChatUIService
    {
        private readonly IGeminiChatService? _chatService;
        private const int MaxRetryAttempts = 3;
        private const int InitialDelayMs = 800;

        public ChatUIService(IGeminiChatService? chatService)
        {
            _chatService = chatService;
        }

        public bool IsAvailable => _chatService != null;

        public async Task<string?> SendMessageWithRetryAsync(string userMessage)
        {
            if (_chatService == null)
                throw new InvalidOperationException("Chat service is not available");

            if (string.IsNullOrWhiteSpace(userMessage))
                return null;

            int delayMs = InitialDelayMs;
            Exception? lastException = null;

            for (int attempt = 1; attempt <= MaxRetryAttempts; attempt++)
            {
                try
                {
                    return await _chatService.ChatAsync(userMessage);
                }
                catch (Exception ex) when (IsTransientError(ex) && attempt < MaxRetryAttempts)
                {
                    lastException = ex;
                    await Task.Delay(delayMs + new Random().Next(0, 200));
                    delayMs *= 2;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            throw lastException ?? new InvalidOperationException("Failed to get chat response after retries");
        }

        private bool IsTransientError(Exception ex)
        {
            if (ex is System.Net.Http.HttpRequestException)
                return true;

            var message = ex.Message?.ToLowerInvariant() ?? string.Empty;
            return message.Contains("high demand") ||
                   message.Contains("rate limit") ||
                   message.Contains("429") ||
                   message.Contains("503") ||
                   message.Contains("timeout");
        }

        public string GetUnavailableMessage() =>
            "⚠️ Gemini Chat Service not available.\nPlease ensure GOOGLE_API_KEY is set.";

        public string GetErrorTitle(Exception ex)
        {
            var msg = ex.Message?.ToLowerInvariant() ?? "";
            if (msg.Contains("high demand") || msg.Contains("rate limit") || msg.Contains("503"))
                return "Service Overloaded";
            return "Chat Error";
        }

        public string GetErrorMessage(Exception ex)
        {
            var msg = ex.Message?.ToLowerInvariant() ?? "";
            if (msg.Contains("high demand") || msg.Contains("rate limit") || msg.Contains("503"))
                return "Chat service is temporarily overloaded. Please try again in a few minutes.";
            return ex.Message ?? "An error occurred while calling the chat service.";
        }
    }
}
