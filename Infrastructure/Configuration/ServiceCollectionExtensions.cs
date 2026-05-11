using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Edamame.Application.Interfaces;
using Edamame.Application.Services;
using Edamame.Domain.Entities;
using Edamame.Domain.Interfaces;
using Edamame.Infrastructure.ExternalServices;
using Edamame.Infrastructure.Persistence;
using Edamame.Presentation.Helpers;
using Edamame.Presentation.Interfaces;
using Edamame.Presentation.Services;

namespace Edamame.Infrastructure.Configuration;
public static class ServiceCollectionExtensions
{
    // registers all application services with DI container
    public static IServiceCollection AddMealTrackerServices(
        this IServiceCollection services,
        string? geminiApiKey = null,
        string? databasePath = null)
    {
        // Resolve Gemini API key
        if (string.IsNullOrWhiteSpace(geminiApiKey))
        {
            geminiApiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
        }

        // persistence
        var connectionFactory = new LiteDbConnectionFactory(databasePath);
        var database = connectionFactory.GetDatabase();
        services.AddSingleton(database);
        services.AddSingleton(connectionFactory);

        // infra repos
        services.AddScoped<IRepository<Meal>>(provider =>
            new LiteDbRepository<Meal>(provider.GetRequiredService<ILiteDatabase>()));

        services.AddScoped<IRepository<Recipe>>(provider =>
            new LiteDbRepository<Recipe>(provider.GetRequiredService<ILiteDatabase>()));

        // domain svcs
        services.AddScoped<IDailyMealAggregator>(provider =>
            new DailyMealAggregator(provider.GetRequiredService<IRepository<Meal>>()));

        // external svcs
        if (!string.IsNullOrWhiteSpace(geminiApiKey))
        {
            services.AddScoped<INutritionAnalysisService>(provider =>
                new GeminiNutritionAnalysisService(geminiApiKey));

            services.AddScoped<IGeminiChatService>(provider =>
                new GeminiChatService(geminiApiKey));
        }
        else
        {
            // use NullChatService when API key unavailable
            services.AddScoped<IGeminiChatService>(_ => new NullChatService());
            
            // throw for nutrition service
            services.AddScoped<INutritionAnalysisService>(provider =>
            {
                throw new InvalidOperationException(
                    "Nutrition Analysis Service requires GOOGLE_API_KEY environment variable.");
            });
        }

        // app svcs
        services.AddScoped<IMealService, MealService>();
        services.AddScoped<MealService>();  // Also register concrete type for backward compatibility

        // presentation svcs
        services.AddScoped<MealUIService>();
        services.AddScoped<DashboardService>();
        services.AddScoped<BmiUIService>();
        services.AddScoped<ChatUIService>();
        services.AddScoped<SettingsService>();
        services.AddScoped<UIRenderingService>();

        // parsing and theme 
        services.AddScoped<IIngredientParser, IngredientParser>();
        services.AddScoped<IColorTheme, DefaultColorTheme>();
        
        // presentation controller
        services.AddScoped<Presentation.Controllers.FormController>();
        services.AddScoped<IFormController>(provider => provider.GetRequiredService<Presentation.Controllers.FormController>());

        return services;
    }
}
