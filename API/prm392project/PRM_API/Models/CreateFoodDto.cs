namespace PRM_API.Models
{
    public class CreateFoodDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? Calories { get; set; }
        public string? Image { get; set; }
    }
}
