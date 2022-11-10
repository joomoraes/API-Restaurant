
namespace Api.Application.Data.Repositories
{

    using Api.Application.Data.Schemas;
    using Api.Application.Domain.Entities;
    using Api.Application.Domain.Enums;
    using Api.Application.Domain.ValueObjects;
    using global::MongoDB.Driver;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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

        public async Task<IEnumerable<Restaurant>> GetAll()
        {
            var restaurant = new List<Restaurant>();

            await _restaurants.AsQueryable().ForEachAsync(d =>
            {
                var r = new Restaurant(d.Id.ToString(), d.Name, d.Kitchen);
                var e = new Address(d.Address.PublicPlace,
                    d.Address.Number,
                    d.Address.City,
                    d.Address.State,
                    d.Address.ZipCode);
                r.AtributeAddress(e);

                restaurant.Add(r);
            });

            return restaurant;
        }
    }
}
