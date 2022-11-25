
namespace Api.Application.Data.Schemas
{
    using global::MongoDB.Bson;
    using global::MongoDB.Bson.Serialization.Attributes;
    using System.Collections.Generic;

    public class RestaurantReviewSchema
    {

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        public string Id { get; set; }
        public double AverageStars { get; set; }
        public List<RestaurantSchema> Restaurant { get; set; }
        public List<ReviewSchema> Reviews { get; set; }

    }
}
