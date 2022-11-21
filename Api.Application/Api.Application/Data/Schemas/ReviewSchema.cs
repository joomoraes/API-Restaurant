

namespace Api.Application.Data.Schemas
{
    using Api.Application.Domain.ValueObjects;
    using global::MongoDB.Bson;
    using global::MongoDB.Bson.Serialization.Attributes;

    public class ReviewSchema
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string RestaurantId { get; set; }
        public int Starts { get; set; }
        public string Comments { get; set; }

    }

    public static class ReviewSchemaExtension
    {
        public static Review ParseToDomain(this ReviewSchema document)
        {
            return new Review(document.Starts, document.Comments);
        }
    }
}
