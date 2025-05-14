using System;
using System.Collections.Generic;

namespace PRM_API.Models
{
    public partial class ChatMessage
    {
        public int MessageId { get; set; }
        public string? Sender { get; set; }
        public string? Text { get; set; }
        public int? UserId { get; set; }
        public DateTime? SentAt { get; set; }

        public virtual User? User { get; set; }
    }
}
