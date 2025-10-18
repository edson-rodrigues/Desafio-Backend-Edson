using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mottu.Application.Interfaces;
using Mottu.Domain.Interfaces;
using Mottu.Infrastructure.Messaging;
using Mottu.Infrastructure.Messaging.Consumers;
using Mottu.Infrastructure.Persistence.MongoDB;
using Mottu.Infrastructure.Persistence.PostgreSQL;
using Mottu.Infrastructure.Persistence.PostgreSQL.Repositories;
using Mottu.Infrastructure.Storage;

namespace Mottu.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // PostgreSQL
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // MongoDB
        services.Configure<MongoDbSettings>(options =>
        {
            var mongoSettings = configuration.GetSection("MongoDbSettings");
            options.ConnectionString = mongoSettings["ConnectionString"] ?? "mongodb://localhost:27017";
            options.DatabaseName = mongoSettings["DatabaseName"] ?? "mottu_events";
            options.EventsCollectionName = mongoSettings["EventsCollectionName"] ?? "motorcycle_events";
        });
        services.AddSingleton<MongoDbContext>();

        // Repositories
        services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
        services.AddScoped<IDeliveryDriverRepository, DeliveryDriverRepository>();
        services.AddScoped<IRentalRepository, RentalRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // MassTransit with RabbitMQ
        services.AddMassTransit(x =>
        {
            x.AddConsumer<MotorcycleRegistered2024Consumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMQ:Host"] ?? "localhost", "/", h =>
                {
                    h.Username(configuration["RabbitMQ:Username"] ?? "guest");
                    h.Password(configuration["RabbitMQ:Password"] ?? "guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        // Event Publisher
        services.AddScoped<IEventPublisher, EventPublisher>();

        // MinIO Storage
        services.Configure<MinIOSettings>(options =>
        {
            var minioSettings = configuration.GetSection("MinIO");
            options.Endpoint = minioSettings["Endpoint"] ?? "localhost:9000";
            options.AccessKey = minioSettings["AccessKey"] ?? "minioadmin";
            options.SecretKey = minioSettings["SecretKey"] ?? "minioadmin";
            options.BucketName = minioSettings["BucketName"] ?? "cnh-images";
            options.UseSSL = bool.Parse(minioSettings["UseSSL"] ?? "false");
        });
        services.AddScoped<IFileStorageService, MinIOFileStorageService>();

        return services;
    }
}

