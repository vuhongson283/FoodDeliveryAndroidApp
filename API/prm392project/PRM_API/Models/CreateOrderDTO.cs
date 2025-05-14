namespace PRM_API.Models
{
    public class CreateOrderDTO
    {
        public string customerName { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
        public string paymentMethod { get; set; }
        public List<OrderDetailsDTO> orderDetails { get; set; }
    }
}
