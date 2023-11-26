using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Taslak.ViewModels
{
    public class LoginViewModel
    {
        
        [Required(ErrorMessage="E-mail is required.")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage ="Password must be at least 6 characters long.")]
        [MaxLength(100,ErrorMessage ="Password cannot be longer than 100 characters.")]
        [Required(ErrorMessage ="Password is required.")]
        public string Password { get; set; }
    }
}