using Google.GenAI;
using Google.GenAI.Types;
using Edamam.Domain.Interfaces;

namespace Edamam.Infrastructure.ExternalServices;

/// Implements IGeminiChatService
/// conversational AI assistant

public class GeminiChatService : IGeminiChatService
{
    private readonly Client _geminiClient;
    private const string MODEL_NAME = "gemini-2.5-flash";
    private readonly List<(string role, string text)> _conversationHistory;

    public GeminiChatService(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("API key cannot be null or empty.", nameof(apiKey));

        _geminiClient = new Client(apiKey: apiKey);
        _conversationHistory = new List<(string, string)>();
    }


    // sends a message to the Gemini chatbot and gets a response
    public async Task<string> ChatAsync(string userMessage)
    {
        return await ChatWithContextAsync(userMessage, null);
    }


    // Sends a message with optional context about nutrition/meals

    public async Task<string> ChatWithContextAsync(string userMessage, string? context = null)
    {
        if (string.IsNullOrWhiteSpace(userMessage))
        {
            throw new ArgumentException("User message cannot be empty.", nameof(userMessage));
        }

        try
        {
            var fullMessage = context != null 
                ? $"{userMessage}\n\nContext:\n{context}" 
                : userMessage;

            System.Diagnostics.Debug.WriteLine($"Gemini Chat Request: {userMessage}");

            // Add user message to history
            _conversationHistory.Add(("user", fullMessage));

            // Build prompt with system instruction and history
            var systemInstruction = @"You are a helpful nutrition and meal planning assistant. You provide accurate, evidence-based advice about nutrition, dietary planning, and healthy eating habits. 
You are friendly, supportive, and encouraging. Keep responses concise but informative.";

            var promptBuilder = new System.Text.StringBuilder();
            promptBuilder.AppendLine(systemInstruction);
            promptBuilder.AppendLine();

            // Add conversation history
            foreach (var (role, text) in _conversationHistory)
            {
                if (role == "user")
                {
                    promptBuilder.AppendLine($"User: {text}");
                }
                else if (role == "assistant")
                {
                    promptBuilder.AppendLine($"Assistant: {text}");
                }
            }

            int maxAttempts = 4;
            int delayMs = 800;
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    var response = await _geminiClient.Models.GenerateContentAsync(
                        model: MODEL_NAME,
                        contents: promptBuilder.ToString()
                    );

                    var responseText = ExtractResponseText(response);

                    // success
                    System.Diagnostics.Debug.WriteLine($"Gemini Chat Response: {responseText}");
                    _conversationHistory.Add(("assistant", responseText));
                    return responseText;
                }
                catch (Exception ex) when (IsTransient(ex) && attempt < maxAttempts)
                {
                    System.Diagnostics.Debug.WriteLine($"Gemini chat transient error (attempt {attempt}): {ex.Message}");
                    await Task.Delay(delayMs + (new Random()).Next(0, 200));
                    delayMs *= 2;
                    continue;
                }
            }

            throw new InvalidOperationException("Gemini Chat API failed after retries.");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error calling Gemini Chat API: {ex.Message}", ex);
        }
    }

    /// clears the conversation history

    public async Task ClearHistoryAsync()
    {
        _conversationHistory.Clear();
        await Task.CompletedTask;
    }

    private static bool IsTransient(Exception ex)
    {
        if (ex is System.Net.Http.HttpRequestException) return true;
        var msg = ex.Message?.ToLowerInvariant() ?? string.Empty;
        if (msg.Contains("high demand") || msg.Contains("rate limit") || msg.Contains("429") || msg.Contains("503") || msg.Contains("timeout"))
            return true;
        return false;
    }

    private static string ExtractResponseText(GenerateContentResponse? response)
    {
        var candidates = response?.Candidates;
        if (candidates == null || candidates.Count == 0)
            throw new InvalidOperationException("Gemini API returned no candidates in response.");

        var parts = candidates[0]?.Content?.Parts;
        if (parts == null || parts.Count == 0)
            throw new InvalidOperationException("Gemini API returned no content parts in response.");

        var responseText = parts[0]?.Text;
        if (string.IsNullOrWhiteSpace(responseText))
            throw new InvalidOperationException("Gemini API returned empty response text.");

        return responseText;
    }
}
