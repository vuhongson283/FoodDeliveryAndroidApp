namespace PRM_API.Models
{
    public class UpdateFoodDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? Calories { get; set; }
        public string? Image { get; set; }
    }
}
