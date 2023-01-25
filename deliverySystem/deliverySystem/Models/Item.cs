using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace deliverySystem.Models
{
    
    public partial class Item
    {

        public Item() {
            Orders = new List<OrderItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Upload Image")]
        [Required(ErrorMessage = "Please choose image to upload.")]
        public string File { get; set; }
        public Category Category { get; set; }

        public virtual ICollection<OrderItem> Orders { get; set; }
    }
}
