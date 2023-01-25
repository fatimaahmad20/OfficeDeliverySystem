using deliverySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace deliverySystem.viewmodel
{
    public class clientviewmodel
    {
        public IEnumerable<User> clientlist { get; set; }
        public User client { get; set; }
    }
} 