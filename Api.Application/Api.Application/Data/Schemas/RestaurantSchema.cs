
namespace Api.Application.Data.Schemas
{
    using Api.Application.Domain.Entities;
    using Api.Application.Domain.Enums;
    using Api.Application.Domain.ValueObjects;
    using global::MongoDB.Bson;
    using global::MongoDB.Bson.Serialization.Attributes;

    public class RestaurantSchema
    {

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public EKitchen Kitchen { get; set; }
        public AddressSchema Address { get; set; }

    }

    public static class RestaurantSchemaExtension
    {
        public static Restaurant ParseToDomain(this RestaurantSchema document)
        {
            var restaurant = new Restaurant(document.Id.ToString(), document.Name, document.Kitchen);
            var address = new Address(document.Address.PublicPlace, document.Address.Number,
                document.Address.City, document.Address.State, document.Address.ZipCode);
            restaurant.AtributeAddress(address);

            return restaurant;
        }
    }
}
