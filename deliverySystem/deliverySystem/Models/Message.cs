namespace deliverySystem.Models
{
    using System;
    
    public partial class Message
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime Time { get; set; }
        public User Sender { get; set; }
    }
}
