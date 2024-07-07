using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FoodieDelight_Api.Data
{
    public class RestaurantRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<RestaurantRepository> _logger;

        public RestaurantRepository(ILogger<RestaurantRepository> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        public async Task AddRestaurantAsync(Restaurant restaurant)
        {
            try
            {
                await _appDbContext.Set<Restaurant>().AddAsync(restaurant);
                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a restaurant.");
                throw;
            }
        }

        public async Task<List<Restaurant>> GetAllRestaurantAsync()
        {
            try
            {
                return await _appDbContext.Restaurants.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all restaurants.");
                throw;
            }
        }

        public async Task<Restaurant?> GetRestaurantById(int id)
        {
            try
            {
                IQueryable<Restaurant> query = _appDbContext.Restaurants;
                var restaurant = await query.FirstOrDefaultAsync(r => r.RestroId == id);

                return restaurant;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving restaurant with ID {id}.");
                throw;
            }
        }

        public async Task UpdateRestaurantAsync(int id, Restaurant model)
        {
            try
            {
                var restaurant = await _appDbContext.Restaurants.Include(r => r.MenuItems).FirstOrDefaultAsync(r => r.RestroId == id);

                if (restaurant == null)
                {
                    throw new Exception("Restaurant Not Found");
                }

                // Update restaurant fields with null checks
                restaurant.Name = model.Name ?? restaurant.Name;
                restaurant.ContactNumber = model.ContactNumber ?? restaurant.ContactNumber;
                restaurant.CuisineType = model.CuisineType ?? restaurant.CuisineType;
                restaurant.EmailAddress = model.EmailAddress ?? restaurant.EmailAddress;
                restaurant.PriceRange = model.PriceRange ?? restaurant.PriceRange;
                restaurant.OperatingHours = model.OperatingHours ?? restaurant.OperatingHours;
                restaurant.Website = model.Website ?? restaurant.Website;
                restaurant.Location = model.Location ?? restaurant.Location;
                restaurant.Description = model.Description ?? restaurant.Description;
                restaurant.AverageRating = model.AverageRating ?? restaurant.AverageRating;

                // Update menu items
                if (model.MenuItems != null)
                {
                    // Remove existing menu items that are not in the new list
                    var itemsToRemove = restaurant.MenuItems.Where(mi => !model.MenuItems.Any(m => m.MenuId == mi.MenuId)).ToList();
                    foreach (var item in itemsToRemove)
                    {
                        restaurant.MenuItems.Remove(item);
                    }

                    // Update or add new menu items
                    foreach (var menuItem in model.MenuItems)
                    {
                        var existingMenuItem = restaurant.MenuItems.FirstOrDefault(mi => mi.MenuId == menuItem.MenuId);
                        if (existingMenuItem != null)
                        {
                            existingMenuItem.Name = menuItem.Name;
                            existingMenuItem.Description = menuItem.Description;
                            existingMenuItem.Price = menuItem.Price;
                        }
                        else
                        {
                            restaurant.MenuItems.Add(new MenuItem
                            {
                                Name = menuItem.Name,
                                Description = menuItem.Description,
                                Price = menuItem.Price,
                            });
                        }
                    }
                }

                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating restaurant with ID {id}.");
                throw;
            }
        }

        public async Task DeleteRestaurant(int id)
        {
            try
            {
                var restaurant = await _appDbContext.Restaurants
                    .Include(r => r.MenuItems) // Include menuItems to load related entities
                    .FirstOrDefaultAsync(r => r.RestroId == id);

                if (restaurant == null)
                {
                    throw new Exception("Restaurant Not Found");
                }

                // Remove menuItems associated with the restaurant
                //_appDbContext.MenuItems.RemoveRange(restaurant.MenuItems);

                // Remove the restaurant itself
                _appDbContext.Restaurants.Remove(restaurant);

                // Save changes
                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting restaurant with ID {id}.");
                throw;
            }
        }
    }
}
