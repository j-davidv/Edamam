# Edamame 🥗

![UML Class Diagram - Edamame Architecture](docs/architecture.svg)

Edamame is a desktop meal-planning and nutrition analysis application. It helps users create meals from recipes, analyze nutritional content, and get simple dietary advice. The project includes a WinForms presentation layer, domain entities for recipes and meals, and services that call AI/third-party nutrition analysis APIs.

## Key ideas 💡

- Create and combine recipes into meals 🍽️
- Calculate calories, macronutrients and other nutrition facts 🔢
- Provide basic dietary classification and guidance 🩺

## Architecture / UML diagram 🏗️

The diagram above shows the class architecture including:
- **Domain Entities**: EntityBase, Meal, Recipe, Ingredient, NutritionalBase, NutritionalMetric, MealType
- **Domain Interfaces**: IRepository<T>, IMealService, IDailyMealAggregator, INutritionAnalysisService, IGeminiChatService, IIngredientParser
- **Application Layer**: MealService, DailyMealAggregator, DashboardService, MealUIService, ChatUIService, BmiUIService, SettingsService
- **Infrastructure Layer**: LiteDbRepository<T>, LiteDbConnectionFactory, GeminiChatService, NullChatService, GeminiNutritionAnalysisService, IngredientParser, ServiceCollectionExtensions
- **Presentation Layer**: IFormController, FormController, MainForm, Program, DashboardData (DTO)

To render the UML diagram directly in the README, download your SVG/PNG into `docs/architecture.svg` (or `docs/architecture.png`) and keep the image path above.

## UI (high level) 🖥️

- Main window with a recipe list and meal builder 🧾
- Nutrition analysis results with summary metrics and simple advice 📊
- Recipe editor and ingredient parser ✍️

## Features ✨

- Create, edit and combine recipes 📝
- Ingredient parsing with quantity/unit normalization 🔎
- Nutrition analysis via AI/third-party service (Gemini / Edamam-style) 🤖
- In-memory caching of recent nutrition lookups for improved performance ⚡

## Tech stack 🛠️

- .NET 10 ⚙️
- WinForms desktop UI (Presentation project) 🪟
- C# for domain, services and infrastructure 🧑‍💻
- Google GenAI client (used in `Infrastructure/ExternalServices`) 🤝

## Getting started 🚀

### Prerequisites ✅

- .NET 10 SDK: https://dotnet.microsoft.com
- Visual Studio Community 2026 (18.4.3) or Visual Studio Code

### Clone the repository 📦

```bash
git clone https://github.com/j-davidv/Edamame.git
cd Edamame
```

### Configuration 🔧

The nutrition analysis service requires an API key for the AI provider. Provide it via environment variable or other configuration.

#### Setting up API Keys 🔑

**Option 1: Environment Variable (Recommended)**

```powershell
# Windows (PowerShell)
$env:GEMINI_API_KEY = "your_gemini_api_key_here"
```

**Option 2: Use user secrets / local configuration**

Place keys in your local configuration or secrets store and load them at startup when constructing the analysis service.

### Build and run ▶️

#### Visual Studio

1. Open `Edamame.sln` in Visual Studio
2. Restore NuGet packages
3. Build the solution (target .NET 10)
4. Set the WinForms project in `Presentation/` as the Startup Project and run (F5)

#### CLI

```powershell
dotnet build
dotnet run --project Presentation/Presentation.csproj
```

## Notes 📝

- The application caches nutrition lookups in-memory; the cache resets when the app exits.
- Ingredient lines are normalized before sending to the AI service to improve parsing and deterministic results.
- The AI client configuration may use `Temperature = 0` for deterministic outputs where applicable.

## Contributing 🤝

Contributions are welcome. Open issues for bugs or feature requests and submit pull requests for proposed changes.

## License 📜

Check the repository root for a `LICENSE` file. If none exists, contact the project owner to confirm licensing.

## Useful files 📁

- `Presentation/` — WinForms UI project
- `Domain/Entities/Recipe.cs` — recipe and ingredient models
- `Infrastructure/ExternalServices/GeminiNutritionAnalysisService.cs` — AI nutrition analysis implementation
- `Infrastructure/Configuration/ServiceCollectionExtensions.cs` — DI and service registration

If you need help building or running the app locally, open an issue with your OS, .NET SDK version and any build errors.