using System;
using System.Collections.Generic;

namespace PRM_API.Models
{
    public partial class User
    {
        public User()
        {
            ChatMessages = new HashSet<ChatMessage>();
            Orders = new HashSet<Order>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Role { get; set; }
        public string Password { get; set; } = null!;

        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
