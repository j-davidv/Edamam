# Edamame

![UML Class Diagram - Edamame Architecture](docs/architecture.svg)

Edamame is a desktop meal-planning and nutrition analysis application. It helps users create meals from recipes, analyze nutritional content, and get simple dietary advice. The project includes a WinForms presentation layer, domain entities for recipes and meals, and services that call AI/third-party nutrition analysis APIs.

## Key ideas

- Create and combine recipes into meals
- Calculate calories, macronutrients and other nutrition facts
- Provide basic dietary classification and guidance

## Architecture / UML diagram

The diagram above shows the complete class architecture including:
- **Domain Entities**: EntityBase, Meal, Recipe, Ingredient, NutritionalBase, NutritionalMetric, MealType
- **Domain Interfaces**: IRepository<T>, IMealService, IDailyMealAggregator, INutritionAnalysisService, IGeminiChatService, IIngredientParser
- **Application Layer**: MealService, DailyMealAggregator, DashboardService, MealUIService, ChatUIService, BmiUIService, SettingsService
- **Infrastructure Layer**: LiteDbRepository<T>, LiteDbConnectionFactory, GeminiChatService, NullChatService, GeminiNutritionAnalysisService, IngredientParser, ServiceCollectionExtensions
- **Presentation Layer**: IFormController, FormController, MainForm, Program, DashboardData (DTO)

## UI (high level)

- Main window with a recipe list and meal builder
- Nutrition analysis results with summary metrics and simple advice
- Recipe editor and ingredient parser

## Features

- Create, edit and combine recipes
- Ingredient parsing with quantity/unit normalization
- Nutrition analysis via AI/third-party service (Gemini / Edamam-style)
- In-memory caching of recent nutrition lookups for improved performance

## Tech stack

- .NET 10
- WinForms desktop UI (Presentation project)
- C# for domain, services and infrastructure
- Google GenAI client (used in `Infrastructure/ExternalServices`)

## Configuration

The nutrition analysis service requires an API key for the AI provider. Provide it via environment variable or other configuration.

### Setting up API Keys

**Option 1: Environment Variable (Recommended)**
```bash
# Windows (Command Prompt)
set GEMINI_API_KEY=your_gemini_api_key_here

# Windows (PowerShell)
$env:GEMINI_API_KEY="your_gemini_api_key_here"

# Linux/macOS
export GEMINI_API_KEY="your_gemini_api_key_here"