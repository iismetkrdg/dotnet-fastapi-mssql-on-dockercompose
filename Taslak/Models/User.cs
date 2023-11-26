using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Taslak.Models
{
    public class User
    {   
        [Key]
        public int UserID { get; set; }
        [Required(ErrorMessage ="Kullanıcı adı zorunludur.")]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage ="Şifre minimum 6 karakter uzunluğunda olmalı.")]
        [MaxLength(100,ErrorMessage ="Şifre maximum 100 karakter uzunluğunda olmalı.")]
        [Required(ErrorMessage ="Şifre zorunludur.")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage ="Şifre minimum 6 karakter uzunluğunda olmalı.")]
        [MaxLength(100,ErrorMessage ="Şifre maximum 100 karakter uzunluğunda olmalı.")]
        [Required(ErrorMessage ="Şifre zorunludur.")]
        [Compare(nameof(Password),ErrorMessage ="Şifreler uyuşmuyor.")]
        public string Repassword { get; set; }
        
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage ="E-mail zorunludur.")]
        public string Email { get; set; }
        public string Role { get; set; }
        public int xTimesUsedAlg { get; set; } = 0;
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public DateTime LastLoginDate { get; set; } = DateTime.Now;
        public DateTime LastLoginDate2 { get; set; } = DateTime.Now;
    }
}