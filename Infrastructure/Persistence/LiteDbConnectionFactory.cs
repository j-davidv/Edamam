using LiteDB;

namespace Edamame.Infrastructure.Persistence;


// managing LiteDB connections


public class LiteDbConnectionFactory
{
    private static ILiteDatabase? _instance;
    private static readonly object _lock = new();
    private readonly string _connectionString;

    public LiteDbConnectionFactory(string? databasePath = null)
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string dbFolder = Path.Combine(appDataPath, "MealTracker");
        
        if (!Directory.Exists(dbFolder))
        {
            Directory.CreateDirectory(dbFolder);
        }

        string dbFile = databasePath ?? Path.Combine(dbFolder, "meals.db");
        _connectionString = $"Filename={dbFile};Connection=shared";
    }

    public ILiteDatabase GetDatabase()
    {
        if (_instance != null) return _instance;

        lock (_lock)
        {
            if (_instance == null)
            {
                _instance = new LiteDatabase(_connectionString);
                ConfigureBsonMapper(_instance);
            }
            return _instance;
        }
    }

    private void ConfigureBsonMapper(ILiteDatabase database)
    {
        var mapper = BsonMapper.Global;

        // Configure nested collection handling
        mapper.Entity<Edamame.Domain.Entities.Meal>()
            .Id(x => x.Id);

        mapper.Entity<Edamame.Domain.Entities.Recipe>()
            .Id(x => x.Id);

        mapper.Entity<Edamame.Domain.Entities.NutritionalMetric>();
    }

    public void CloseDatabase()
    {
        lock (_lock)
        {
            _instance?.Dispose();
            _instance = null;
        }
    }
}
