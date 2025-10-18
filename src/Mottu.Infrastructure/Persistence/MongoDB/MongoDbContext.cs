using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace Mottu.Infrastructure.Persistence.MongoDB;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string EventsCollectionName { get; set; } = "motorcycle_events";
}

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<MotorcycleEventDocument> MotorcycleEvents =>
        _database.GetCollection<MotorcycleEventDocument>("motorcycle_events_2024");
}

public class MotorcycleEventDocument
{
    public string Id { get; set; } = string.Empty;
    public string MotorcycleId { get; set; } = string.Empty; // Changed from Guid to string
    public string Identifier { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Model { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

