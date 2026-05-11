namespace Edamame.Presentation.Models
{
    
    // manages chat session state and history
    
    public class ChatSession
    {
        public List<(string Message, bool IsUser, DateTime Timestamp)> History { get; } = new();

        public void AddMessage(string message, bool isUser)
        {
            History.Add((message, isUser, DateTime.Now));
        }

        public void Clear()
        {
            History.Clear();
        }

        public string GetWelcomeMessage() =>
            "👋 Hi! I'm your AI Nutrition Coach.\n\n" +
            "💡 You can ask me about:\n" +
            "• Macronutrient breakdowns\n" +
            "• Nutrition goals\n" +
            "• Meal suggestions\n" +
            "• Health tips\n\n" +
            "Let's make your meals healthier!";
    }
}
