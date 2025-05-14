using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRM_API.Models;

namespace PRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly FoodDeliveryContext _foodDeliveryContext;
        public FoodController(FoodDeliveryContext foodDeliveryContext)
        {
            _foodDeliveryContext = foodDeliveryContext;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNewFood([FromBody] CreateFoodDto createFoodDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(createFoodDto.Name) || createFoodDto.Price <= 0 || createFoodDto.Calories < 0)
                {
                    return BadRequest("Name is required, price and calories must be greater than zero.");
                }


                var food = new Food
                {
                    Name = createFoodDto.Name,
                    Description = createFoodDto.Description,
                    Price = createFoodDto.Price,
                    Calories = createFoodDto.Calories,
                    Image = createFoodDto.Image
                };
                _foodDeliveryContext.Foods.Add(food);
                await _foodDeliveryContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetFoodById), new { id = food.FoodId }, food);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateFood(int id, [FromBody] UpdateFoodDto foodDto)
        {
            var food = await _foodDeliveryContext.Foods.FindAsync(id);
            if (food == null)
            {
                return NotFound("Food not found.");
            }


            // Cập nhật các trường nếu có dữ liệu từ DTO
            if (!string.IsNullOrWhiteSpace(foodDto.Name)) food.Name = foodDto.Name;
            if (foodDto.Description != null) food.Description = foodDto.Description;
            if (foodDto.Price.HasValue) food.Price = foodDto.Price.Value;
            if (foodDto.Calories.HasValue) food.Calories = foodDto.Calories.Value;
            if (foodDto.Image != null) food.Image = foodDto.Image;

            _foodDeliveryContext.Update(food);
            await _foodDeliveryContext.SaveChangesAsync();


            return Ok(food);
        }




  

        [HttpGet("customer")]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoods(
        [FromQuery] int pageIndex,
        [FromQuery] int pageSize,
        [FromQuery] string? keyword = "")
        {
            var query = _foodDeliveryContext.Foods.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(f => f.Name.Contains(keyword));
            }

            var totalItems = await query.CountAsync();
            var foods = await query
                .Skip(Math.Max(0, (pageIndex - 1) * pageSize))
                .Take(pageSize).Select(x => new
                {
                    FoodID = x.FoodId.ToString(),
                    x.Name,
                    x.Description,
                    Price = Convert.ToDouble(x.Price),
                    x.Calories,
                    x.Image
                })
                .ToListAsync();

            Response.Headers.Add("X-Total-Count", totalItems.ToString());

            return Ok(foods);
        }

        [HttpGet("{id}")]
        public IActionResult GetFoodById(int id)
        {
            var food = _foodDeliveryContext.Foods.FirstOrDefault(f => f.FoodId == id);
            if (food == null)
            {
                return NotFound(new { message = "Food not found" });
            }
            return Ok(food);
        }
    }
}


