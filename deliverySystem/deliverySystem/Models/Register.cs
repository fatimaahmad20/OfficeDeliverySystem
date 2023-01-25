namespace deliverySystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Register
    {
        public User User { get; set; }
        [Required(ErrorMessage = "Department is required.")]
        public int SelectedDepartmentId { get; set; }
        public List<Department> Departments { get; set; }
    }
}
