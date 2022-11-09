
namespace Api.Application.Data.Schemas
{

    using Api.Application.Domain.Enums;
    using global::MongoDB.Bson;
    using global::MongoDB.Bson.Serialization.Attributes;

    public class RestaurantSchema
    {

        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public EKitchen Kitchen { get; set; }
        public AddressSchema Address { get; set; }
    }
}
