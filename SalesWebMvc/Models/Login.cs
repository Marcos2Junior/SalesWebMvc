using SalesWebMvc.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace SalesWebMvc.Models
{
    public class Login
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} size should be between {2} and {1}")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime BithDate { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [DataType(DataType.Password)]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "{0} size should be between {2} and {1}")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public Permission Permission { get; set; }

        public Login()
        { }

        public Login(int id, string name, string email, DateTime bithDate, string password, Permission permission)
        {
            Id = id;
            Name = name;
            Email = email;
            BithDate = bithDate;
            Password = password;
            Permission = permission;
        }
    }
}
