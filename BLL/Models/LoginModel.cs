using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL.Models
{
    public class LoginModel
    {
        [EmailAddress(ErrorMessage = "Please, enter valid email address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password {0} must have min {2} and max {1} characters.", MinimumLength = 5)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
