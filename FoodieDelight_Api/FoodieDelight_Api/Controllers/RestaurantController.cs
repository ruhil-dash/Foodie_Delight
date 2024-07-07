using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodieDelight_Api.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FoodieDelight_Api.Controllers
{
    [Route("api/[controller]")]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantRepository _restaurantRepository;

        public RestaurantController(RestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }


        [HttpPost]
        public async Task<ActionResult> AddRestaurant([FromBody] Restaurant model)
        {
            if (model == null)
            {
                return BadRequest("Invalid data.");
            }
            await _restaurantRepository.AddRestaurantAsync(model);
            return Ok();

        }

        [HttpGet]
        public async Task<ActionResult> GetRestaurantList()
        {
            var restauranList = await _restaurantRepository.GetAllRestaurantAsync();
            return Ok(restauranList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetRestaurantById([FromRoute] int id)
        {
            try
            {
                var restaurant = await _restaurantRepository.GetRestaurantById(id);

                if (restaurant == null)
                {
                    return NotFound(); // Return 404 if restaurant with the given ID is not found
                }

                return Ok(restaurant); // Return 200 with the restaurant object
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error"); // Return 500 if an exception occurs
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRestaurantList([FromRoute] int id, [FromBody] Restaurant model)
        {
             await _restaurantRepository.UpdateRestaurantAsync(id,model);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRestaurant([FromRoute] int id)
        {
            await _restaurantRepository.DeleteRestaurant(id);
            return Ok();
        }
       
        
    }
}

