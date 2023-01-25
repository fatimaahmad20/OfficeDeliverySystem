namespace deliverySystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="First Name is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Birthdate is required.")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Birthdate { get; set; }
        [Required(ErrorMessage = "Office is required.")]
        public string Office { get; set; }
        [Required(ErrorMessage = "Floor must be greater than 0.")]
        [Range(1, 99, ErrorMessage = "Block must be greater than 0.")]
        public int Floor { get; set; }
        public Department Department { get; set; }
        [Required(ErrorMessage = "Block must be greater than 0.")]
        [Range(1, 99, ErrorMessage = "Block must be greater than 0.")]
        public int Block { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Role is required.")]
        public Role Role { get; set; }
        public Category? Category { get; set; }
        public bool DeliveryFree { get; set; }
    }
}
