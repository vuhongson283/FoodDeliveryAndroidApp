using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRM_API.Models;
using System.Net;
using System.Security.Claims;

namespace PRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class orderController : ControllerBase
    {
        private readonly FoodDeliveryContext _context;
        public orderController(FoodDeliveryContext foodDeliveryContext)
        {
            _context = foodDeliveryContext;
        }
        [HttpGet("customerID")]
        [Authorize]

        public async Task<IActionResult> GetOrderList([FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string? keyword = "")
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdClaim.Value);

            var orders = await _context.Orders
     .Where(o => o.CustomerId == userId)
     .Join(_context.OrderDetails, o => o.OrderId, od => od.OrderId, (o, od) => new { o, od })
     .Join(_context.Foods, j => j.od.FoodId, f => f.FoodId, (j, f) => new { j.o, f })
     .Where(t => string.IsNullOrEmpty(keyword) || t.f.Name.ToLower().Contains(keyword.ToLower()))
     .Select(t => t.o) 
     .Distinct()
     .OrderByDescending(o => o.OrderId)
     .Skip(Math.Max(0, (pageIndex - 1) * pageSize))
     .Take(pageSize)
     .Select(o => new
     {
         OrderID = o.OrderId.ToString(),         
         TotalPrice = o.TotalPrice.ToString(),   
         o.OrderStatus,
         CreatedAt = ((DateTime)o.CreatedAt).ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'"),
         cusName = o.CustomerName,
         phone = o.PhoneNumber,
         address = o.Address,
     })
     .ToListAsync();

            return Ok(orders);
        }
        // lay order role admin
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrders([FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string? keyword = "")
        {
            
            var orders = await _context.Orders
     
     .Join(_context.OrderDetails, o => o.OrderId, od => od.OrderId, (o, od) => new { o, od })
     .Join(_context.Foods, j => j.od.FoodId, f => f.FoodId, (j, f) => new { j.o, f })
     .Where(t => string.IsNullOrEmpty(keyword) || t.f.Name.ToLower().Contains(keyword.ToLower()))
     .Select(t => t.o)
     .Distinct()
     .OrderByDescending(o => o.OrderId)
     .Skip(Math.Max(0, (pageIndex - 1) * pageSize))
     .Take(pageSize)
     .Select(o => new
     {
         OrderID = o.OrderId.ToString(),
         TotalPrice = o.TotalPrice.ToString(),
         o.OrderStatus,
         CreatedAt = ((DateTime)o.CreatedAt).ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'"),
         cusName=o.CustomerName,
         phone = o.PhoneNumber,
         address = o.Address,
     })
     .ToListAsync();

            return Ok(orders);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO orderDTO)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdClaim.Value);
            if (orderDTO == null || orderDTO.orderDetails == null || orderDTO.orderDetails.Count == 0)
            {
                return BadRequest("Invalid order data");
            }

            var order = new Order
            {
                CustomerId = userId,
                CustomerName = orderDTO.customerName,
                PhoneNumber = orderDTO.phoneNumber,
                Address = orderDTO.address,
                PaymentMethod = orderDTO.paymentMethod,
                OrderDetails = new List<OrderDetail>(),
                CreatedAt = DateTime.UtcNow,
                OrderStatus = "Pending"
            };

            decimal totalPrice = 0;

            foreach (var detail in orderDTO.orderDetails)
            {
                var food = await _context.Foods.FindAsync(int.Parse(detail.foodID));
                if (food == null)
                {
                    return BadRequest($"Food ID {detail.foodID} not found.");
                }

                decimal itemTotal = food.Price * detail.quantity; // Giá món ăn * số lượng
                totalPrice += itemTotal;

                order.OrderDetails.Add(new OrderDetail
                {
                    FoodId = food.FoodId,
                    Quantity = detail.quantity,
                    Price = food.Price // Lưu giá món ăn vào OrderDetail
                });
            }

            order.TotalPrice = totalPrice;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(order);
        }

        [HttpGet("{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Food) // Lấy thông tin món ăn
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }

            var orderDetailsList = order.OrderDetails.Select(od => new
            {
                foodId = od.FoodId.ToString(),
                name = od.Food.Name,
                quantity = od.Quantity,
                description=od.Food.Description,
                price = Convert.ToDouble(od.Price),
                calories=od.Food.Calories,
                image=od.Food.Image,
                
            }).ToList();

            return Ok(orderDetailsList);
        }
        [HttpPut("{orderId}/{status}")]
        [Authorize]
        public async Task<IActionResult> ChangeStatusOrder(int orderId, string status)
        {
            try
            {
                var o = await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == orderId);
                if (o == null)
                {
                    return NotFound();
                }
                else
                {
                    o.OrderStatus = status;
                    await _context.SaveChangesAsync();

                }

                return Ok(o);
            }catch (Exception ex) { return  BadRequest(ex.Message); }  
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound(new { message = "Không tìm thấy đơn hàng" });
            }

            if (order.OrderStatus != "Pending")
            {
                return BadRequest(new { message = "Chỉ có thể xóa đơn hàng ở trạng thái Pending" });
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đơn hàng đã được xóa thành công" });
        }

    }
}
