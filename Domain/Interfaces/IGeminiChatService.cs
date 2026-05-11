namespace Edamame.Domain.Interfaces;

/// interface for Gemini chat service
/// defines contract for user AI interactions

public interface IGeminiChatService
{
    /// Sends a message to the Gemini chatbot and gets a response
    /// <param name="userMessage">The user's message or question</param>
    /// <returns>The AI's response as a string</returns>
    Task<string> ChatAsync(string userMessage);

    /// Sends a message with context about nutrition/meals
    /// <param name="userMessage">The user's message or question.</param>
    /// <param name="context">Optional context about the meal/nutrition data</param>
    /// <returns>The AI's response as a string.</returns>
    Task<string> ChatWithContextAsync(string userMessage, string? context = null);

    /// Clears the conversation history
    Task ClearHistoryAsync();
}
