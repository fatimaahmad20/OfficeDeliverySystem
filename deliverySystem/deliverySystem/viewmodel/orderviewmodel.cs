using deliverySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace deliverySystem.viewmodel
{
    public class orderviewmodel
    {
        public User client { get; set; }
        public Item order { get; set; }

        public IEnumerable<Item> orderlist { get; set; }
    }
}