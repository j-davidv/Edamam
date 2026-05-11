using Edamame.Domain.Interfaces;

namespace Edamame.Infrastructure.ExternalServices;
public class NullChatService : IGeminiChatService
{
    private const string UnavailableMessage = 
        "⚠️ Chat service is not available.\nPlease configure GOOGLE_API_KEY environment variable.";

    public bool IsAvailable => false;

    public async Task<string> ChatAsync(string userMessage)
    {
        // Return user-friendly message when down
        return await Task.FromResult(UnavailableMessage);
    }

    public async Task<string> ChatWithContextAsync(string userMessage, string? context = null)
    {
        // Return user-friendly message for context-aware chat
        return await Task.FromResult(UnavailableMessage);
    }

    public async Task ClearHistoryAsync()
    {
        // No-op: no history to clear for null service
        await Task.CompletedTask;
    }
}
