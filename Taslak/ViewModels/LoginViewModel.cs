using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Taslak.ViewModels
{
    public class LoginViewModel
    {
        
        [Required(ErrorMessage="Kullanıcı adı zorunludur.")]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage ="Şifre minimum 6 karakter uzunluğunda olmalı.")]
        [MaxLength(100,ErrorMessage ="Şifre maximum 100 karakter uzunluğunda olmalı.")]
        [Required(ErrorMessage ="Şifre zorunludur.")]
        public string Password { get; set; }
    }
}