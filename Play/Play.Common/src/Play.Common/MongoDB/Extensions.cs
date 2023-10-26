using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Common.Settings;
using Play.Common.MongoDB;
using System.Diagnostics;

namespace Play.Common.MongoDB
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                if (configuration != null)
                {

                    var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                    var mongoDbSettings = configuration.GetSection(nameof(MongoDBSettings)).Get<MongoDBSettings>();

                    if (mongoDbSettings != null)
                    {
                        var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
                        if (serviceSettings != null)
                        {
                            return mongoClient.GetDatabase(serviceSettings.ServiceName);
                        }
                        else
                        {
                            throw new Exception($"Could not retrieve Configuration {nameof(ServiceSettings)}");
                        }
                    }
                    else
                    {
                        throw new Exception($"Could not retrieve Configuration {nameof(MongoDBSettings)}");
                    }
                }
                else
                {
                    throw new Exception("Could not retrieve object tipe IConfiguration");
                }
            });

            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T : IEntity
        {
            services.AddSingleton<IRepository<T>>(serviceProvider =>
            {
                var database = serviceProvider.GetService<IMongoDatabase>();
                if (database != null)
                {
                    return new MongoRepository<T>(database, collectionName);
                }
                else
                {
                    throw new Exception("No database (IMongoDatabase) created from IServiceCollection");
                }
            });

            return services;
        }
    }
}