
namespace Api.Application.Controllers
{
    using Api.Application.Controllers.Inputs;
    using Api.Application.Controllers.Outputs;
    using Api.Application.Data.Repositories;
    using Api.Application.Domain.Entities;
    using Api.Application.Domain.Enums;
    using Api.Application.Domain.ValueObjects;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Routing;
    using MongoDB.Bson;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    [ApiController]
    public class RestaurantController : Controller
    {

        private readonly RestaurantRepository _restaurantRepository;

        public RestaurantController(RestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        [HttpPost("restaurant")]
        public IActionResult IncludeRestaurant([FromBody] IncluseRestaurant incluseRestaurant)
        {
            var kitchen = EKitchenHelper.ParseInt(incluseRestaurant.Kitchen);

            var restaurant = new Restaurant(
                incluseRestaurant.Name,
                kitchen);

            var address = new Address(
                incluseRestaurant.PublicReplace,
                incluseRestaurant.Number,
                incluseRestaurant.City,
                incluseRestaurant.State,
                incluseRestaurant.ZipCode);

            restaurant.AtributeAddress(address);

            if (!restaurant._Validate())
            {
                return BadRequest(
                    new
                    {
                        errors = restaurant.ValidationResult.Errors.Select(_ => _.ErrorMessage)
                    });
            }

            _restaurantRepository.Insert(restaurant);

            return Ok(
                    new
                    {
                        data = "Successful! Insert new restaurant"
                    }
                );
        }

        [HttpGet("restaurant/all")]
        public async Task<ActionResult> GetAllRestaurants()
        {
            var restaurants = await _restaurantRepository.GetAll();

            var list = restaurants.Select(_ => new RestaurantList
            {
                Id = _.Id,
                Name = _.Name,
                Kitchen = (int)_.Kitchen,
                City = _.Address.City
            });

            return Ok(
                new
                {
                    data = list
                });
        }

        [HttpGet("restaurant/{id}")]
        public ActionResult GetRestaurantById(string id)
        {
            var restaurant = _restaurantRepository.GetById(id);

            if (restaurant == null)
                return NotFound();

            var view = new RestaurantView
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Kitchen = (int)restaurant.Kitchen,
                Address = new AddressView
                {
                    PublicPlace = restaurant.Address.Publicplace,
                    Number = restaurant.Address.Number,
                    City = restaurant.Address.City,
                    State = restaurant.Address.State,
                    ZipCode = restaurant.Address.ZipCode
                }
            };

            return Ok(
                new
                {
                    data = view
                });
        }

        [HttpPut("restaurant")]
        public ActionResult UpdateRestaurant([FromBody] RestaurantUpdateComplet restaurantUpdateComplet)
        {
            var restaurant = _restaurantRepository.GetById(restaurantUpdateComplet.Id);

            if (restaurant == null)
                return NotFound();

            var kitchen = EKitchenHelper.ParseInt(restaurantUpdateComplet.Kitchen);
            restaurant = new Restaurant(restaurantUpdateComplet.Id, restaurantUpdateComplet.Name, kitchen);
            var address = new Address(
                restaurantUpdateComplet.Publicplace,
                restaurantUpdateComplet.Number,
                restaurantUpdateComplet.City,
                restaurantUpdateComplet.State,
                restaurantUpdateComplet.ZipCode);

            restaurant.AtributeAddress(address);

            if (!restaurant._Validate())
            {
                return BadRequest(
                    new
                    {
                        errors = restaurant.ValidationResult.Errors.Select(_ => _.ErrorMessage)
                    }); ;
            }

            if (!_restaurantRepository.UpdateComplet(restaurant))
            {
                return BadRequest(
                    new
                    {
                        data = "None documents was update!"
                    });
            }

            return Ok(
                new
                {
                    data = "Restaurant was update successful"
                });
        }

        [HttpPatch("restaurant/{id}")]
        public ActionResult UpdateKitchen(string id, [FromBody] RestaurantUpdateParcial restaurantUpdateParcial)
        {
            var restaurant = _restaurantRepository.GetById(id);

            if (restaurant == null)
                return NotFound();

            var kitchen = EKitchenHelper.ParseInt(restaurantUpdateParcial.Kitchen);

            if (!_restaurantRepository.UpdateKitchen(id, kitchen))
            {
                return BadRequest(new
                {
                    errors = "None documento was update"
                });
            }

            return Ok(new
            {
                data = "Kitchen was update successful"
            });
        }

        [HttpGet("restaurant")]
        public ActionResult GetByName([FromQuery] string name)
        {
            var restaurant = _restaurantRepository.GetByName(name);

            var list = restaurant.Select(_ => new RestaurantList
            {
                Id = _.Id,
                Name = _.Name,
                Kitchen = (int)_.Kitchen,
                City = _.Address.City
            });

            return Ok(new
            {
                data = list
            });
        }

        [HttpPatch("restaurant/{id}/review")]
        public ActionResult ReviewRestaurant(string id, [FromBody] ReviewInclud reviewInclud)
        {
            var restaurant = _restaurantRepository.GetById(id);

            if (restaurant == null)
                return NotFound();

            var review = new Review(reviewInclud.Stars, reviewInclud.Comments);

            if (!review._Validate())
            {
                return BadRequest(new
                {
                    errors = review.ValidationResult.Errors.Select(_ => _.ErrorMessage)
                });
            }

            _restaurantRepository.Review(id, review);

            return Ok(
                new {
                    data = "Restaurant review with success"
                }
                );
        }

        [HttpGet("restaurant/top3")]
        public async Task<ActionResult> GetTop3Restaurant()
        {
            var top3 = await _restaurantRepository.GetTop3();

            var list = top3.Select(_ => new RestaurantTop3
            {
                Id = _.Key.Id,
                Name = _.Key.Name,
                Kitchen = (int)_.Key.Kitchen,
                City = _.Key.Address.City,
                Stars = (int)_.Value
            });

            return Ok(new
            {
                data = list
            });
        }

        [HttpDelete("restaurant/{id}")]
        public ActionResult Remove(string id)
        {
            var restaurant = _restaurantRepository.GetById(id);

            if (restaurant == null)
                return NotFound();

            (var resultRestaurantRemoved, var resultReviewRemoved) = _restaurantRepository.Remove(id);

            return Ok(new
            {
                data = $"total of exclude: {resultRestaurantRemoved} restaurant with {resultReviewRemoved} reviews"
            });
        }

        [HttpGet("restaurant/text")]
        public async Task<ActionResult> GetRestaurantByText([FromQuery] string text)
        {
            var restaurant = await _restaurantRepository.GetSearchText(text);

            var list = restaurant.ToList().Select(_ => new RestaurantList 
            {
                Id = _.Id,
                Name = _.Name,
                Kitchen = (int)_.Kitchen,
                City = _.Address.City
            });

            return Ok(
                new
                {
                    data = list
                }); 

        }

        [HttpGet("restaurant/top3-lookup")]
        public async Task<ActionResult> GetTop3Restaurant_WithLookUp()
        {
            var top3 = await _restaurantRepository.GetTop3_WithLookup();

            var list = top3.Select(_ => new RestaurantTop3
            {
                Id = _.Key.Id,
                Name = _.Key.Name,
                Kitchen = (int) _.Key.Kitchen,
                City = _.Key.Address.City,
                Stars = (int) _.Value
            });

            return Ok(new
            {
                data = list
            });
        }
    }
}
