namespace Api.Application.Data
{
    using Api.Application.Data.Schemas;
    using Api.Application.Domain.Entities;
    using Api.Application.Domain.Enums;
    using global::MongoDB.Bson;
    using global::MongoDB.Bson.Serialization;
    using global::MongoDB.Bson.Serialization.Serializers;
    using global::MongoDB.Driver;
    using Microsoft.Extensions.Configuration;
    using System;

    public class MongoDB
    {
        public IMongoDatabase DB { get; }


       public MongoDB(IConfiguration configuration)
        {
            try
            {
                var cliente = new MongoClient(configuration.GetConnectionString("MongoDB"));
                DB = cliente.GetDatabase(configuration["NameDB"]);
                MapClasses();
            }
            catch (Exception ex)
            {

                throw new MongoException("We can't connection with MongoDB ", ex);
            }
        }

        private void MapClasses()
        {
            if(!BsonClassMap.IsClassMapRegistered(typeof(RestaurantSchema)))
            {
                BsonClassMap.RegisterClassMap<RestaurantSchema>(i =>
                {
                    i.AutoMap();
                    i.MapMember(c => c.Id);
                    i.MapMember(c => c.Kitchen).SetSerializer(new EnumSerializer<EKitchen>(BsonType.Int32));
                    i.SetIgnoreExtraElements(true);
                });
            }
        }
    }
}
