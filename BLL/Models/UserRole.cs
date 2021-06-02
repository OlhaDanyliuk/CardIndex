using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL.Models
{
    public class UserRole
    {
        [Required(ErrorMessage = "This field is required")]
        [StringLength(100, ErrorMessage = "Name length can't be more than 100.")]
        [DataType(DataType.Text, ErrorMessage = "Please, enter correct role name.")]
        [RegularExpression("^[A-Z, a-z]*$", ErrorMessage = "Please, enter correct role name.")]
        public string RoleName { get; set; }
    }
}
