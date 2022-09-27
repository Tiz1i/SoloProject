#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace SoloProject.Models;
using System.ComponentModel.DataAnnotations.Schema;


    public class LoginUser
    {
        [Required(ErrorMessage="Email is required")]
        [EmailAddress(ErrorMessage="Please enter a valid Email")]
        public string LoginEmail { get; set; }

        [Required(ErrorMessage="Password is required")]
        [MinLength(8, ErrorMessage="Password must be 8 or more characters")]
        [DataType("Password")]
        public string? LoginPassword { get; set; }

    }
