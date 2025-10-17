using MassTransit;
using Microsoft.Extensions.Logging;
using Mottu.Domain.Events;
using Mottu.Infrastructure.Persistence.MongoDB;

namespace Mottu.Infrastructure.Messaging.Consumers;

public class MotorcycleRegistered2024Consumer : IConsumer<MotorcycleRegisteredEvent>
{
    private readonly MongoDbContext _mongoContext;
    private readonly ILogger<MotorcycleRegistered2024Consumer> _logger;

    public MotorcycleRegistered2024Consumer(
        MongoDbContext mongoContext,
        ILogger<MotorcycleRegistered2024Consumer> logger)
    {
        _mongoContext = mongoContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<MotorcycleRegisteredEvent> context)
    {
        var message = context.Message;

        // Only process motorcycles from year 2024
        if (message.Year != 2024)
        {
            _logger.LogInformation("Motorcycle {MotorcycleId} is not from 2024, skipping...", message.MotorcycleId);
            return;
        }

        _logger.LogInformation("Processing motorcycle from 2024: {MotorcycleId}", message.MotorcycleId);

        var document = new MotorcycleEventDocument
        {
            Id = Guid.NewGuid().ToString(),
            MotorcycleId = message.MotorcycleId,
            Identifier = message.Identifier,
            Year = message.Year,
            Model = message.Model,
            LicensePlate = message.LicensePlate,
            CreatedAt = message.OccurredOn
        };

        await _mongoContext.MotorcycleEvents.InsertOneAsync(document);

        _logger.LogInformation("Motorcycle 2024 event stored in MongoDB: {MotorcycleId}", message.MotorcycleId);
    }
}

