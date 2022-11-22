
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
        IMongoCollection<ReviewSchema> _review;

        public RestaurantRepository(MongoDB mongoDB)
        {
            _restaurants = mongoDB.DB.GetCollection<RestaurantSchema>("restaurant");
            _review = mongoDB.DB.GetCollection<ReviewSchema>("review");
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

        public IEnumerable<Restaurant> GetByName(string name)
        {
            var restaurant = new List<Restaurant>();

            _restaurants.AsQueryable()
                .Where(_ => _.Name.ToLower().Contains(name.ToLower()))
                .ToList()
                .ForEach(d => restaurant.Add(d.ParseToDomain()));

            return restaurant;
        }
        public void Review(string restaurantId, Review review)
        {
            var document = new ReviewSchema
            {
                RestaurantId = restaurantId,
                Starts = review.Starts,
                Comments = review.Comments
            };

            _review.InsertOne(document);
        }

        public async Task<Dictionary<Restaurant, double>> GetTop3()
        {
            var ret = new Dictionary<Restaurant, double>();

            var top3 = _review.Aggregate()
                .Group(_ => _.RestaurantId, g => new
                {
                    RestaurantId = g.Key, 
                    AverageStars = g.Average(a => a.Starts)
                })
                .SortByDescending(_ => _.AverageStars)
                .Limit(3);

            await top3.ForEachAsync(_ =>
            {
                var restaurant = GetById(_.RestaurantId);

                _review.AsQueryable()
                .Where(a => a.RestaurantId == _.RestaurantId)
                .ToList()
                .ForEach(a => restaurant.ReviewInsert(a.ParseToDomain()));

                ret.Add(restaurant, _.AverageStars);
            });

            return ret;
        }

        public (long, long) Remove(string restaurantId)
        {
            var resultReviews = _review.DeleteMany(_ => _.RestaurantId == restaurantId);
            var resultRestaurant = _restaurants.DeleteOne(_ => _.Id == restaurantId);

            return (resultRestaurant.DeletedCount, resultReviews.DeletedCount);
        }

    }
}
