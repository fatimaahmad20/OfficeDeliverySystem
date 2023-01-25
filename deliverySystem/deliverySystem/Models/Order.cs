using System;
using System.Collections.Generic;

namespace deliverySystem.Models
{
    public partial class Order
    {

        public Order() { 
            Items = new List<OrderItem>();
        }

        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public OrderState CafeteriaState { get; set; }
        public OrderState PrinterState { get; set; }
        public User User { get; set; }
        public User CafeteriaDeliveryMan { get; set; }
        public User PrinterDeliveryMan { get; set; }
        public virtual ICollection<OrderItem> Items { get; set; }

    }
}