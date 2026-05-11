global using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Edamame.Infrastructure.Configuration;
using Edamame.Presentation.Interfaces;

namespace Edamame
{
    internal static class Program
    {
        //  THE MAINNN
        [STAThread]
        static void Main()
        {
            // initialize dependency injection container
            var services = new ServiceCollection();

            // load configuration from appsettings.json (GEMINI API KEY)
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            // register Google Gemini API credentials (from environment variables or appsettings.json)
            string? geminiApiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY") 
                ?? config["GeminiApiKey"];
            services.AddMealTrackerServices(geminiApiKey);

            var serviceProvider = services.BuildServiceProvider();

            // create and run the main form 
            var controller = serviceProvider.GetRequiredService<IFormController>();
            var form = new MainForm(controller);
            RunWindowsFormsApp(form);
        }

        private static void RunWindowsFormsApp(Form mainForm)
        {
            global::System.Windows.Forms.Application.Run(mainForm);
        }
    }
}