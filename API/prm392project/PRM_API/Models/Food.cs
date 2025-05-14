using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PRM_API.Models
{
    public partial class Food
    {
        public Food()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }
        [JsonProperty("FoodID")]
        public int FoodId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? Calories { get; set; }
        public string? Image { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
