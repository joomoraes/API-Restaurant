
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
    using System.Linq;

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

        public Restaurant GetById(string id)
        {
            var document = _restaurants.AsQueryable().FirstOrDefault(_ => _.Id == id);

            if (document == null)
                return null;

            return document.ParseToDomain();
        }

        public bool UpdateComplet(Restaurant restaurant)
        {
            var document = new RestaurantSchema
            {
                Id = restaurant.Id,
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

            var result = _restaurants.ReplaceOne(_ => _.Id == document.Id, document);

            return result.ModifiedCount > 0;
        }

        public bool UpdateKitchen(string id, EKitchen kitchen)
        {
            var update = Builders<RestaurantSchema>.Update.Set(_ => _.Kitchen, kitchen);

            var result = _restaurants.UpdateOne(_ => _.Id == id, update);

            return result.ModifiedCount > 0;
        }

    }
}
