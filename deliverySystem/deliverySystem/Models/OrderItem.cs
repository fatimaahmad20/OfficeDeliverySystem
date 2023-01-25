using Microsoft.Ajax.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace deliverySystem.Models
{
    
    public partial class OrderItem
    {
        [Key, Column(Order = 0)]
        public int OrderId { get; set; }
        [Key, Column(Order = 1)]
        public int ItemId { get; set; }

        public int Quantity { get; set; }

        public virtual Order Order { get; set; }
        public virtual Item Item { get; set; }

    }
}
