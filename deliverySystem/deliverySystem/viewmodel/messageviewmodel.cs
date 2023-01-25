using deliverySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace deliverySystem.viewmodel
{
    public class messageviewmodel
    {
        public User client { get; set; }
        public Message Message { get; set; }

        public IEnumerable<Message> messagelist { get; set; }
    }
}