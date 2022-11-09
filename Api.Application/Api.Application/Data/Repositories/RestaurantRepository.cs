
namespace Api.Application.Data.Repositories
{

    using Api.Application.Data.Schemas;
    using Api.Application.Domain.Entities;
    using global::MongoDB.Driver;

    public class RestaurantRepository 
    {
        IMongoCollection<RestaurantSchema> _restaurants;

        public RestaurantRepository(MongoDB mongoDB)
        {
            _restaurants = mongoDB.DB.GetCollection<RestaurantSchema>("restaurant");
        }

        public void Insert(Restaurant restaurant)
        {
            var document = new RestaurantSchema
            {
                Name = restaurant.Name,
                Kitchen = restaurant.Kitchen,
                Address = new AddressSchema
                {
                    PublicPlace = restaurant.Address.Publicplace,
                    Number = restaurant.Address.Number,
                    City = restaurant.Address.City,
                    ZipCode = restaurant.Address.ZipCode,
                    State = restaurant.Address.State
                }
            };

            _restaurants.InsertOne(document);
        }
    }
}
