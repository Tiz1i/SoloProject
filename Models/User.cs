#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace SoloProject.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Display(Name = "First Name")]
    [Required(ErrorMessage = "Name is required")]
    [MinLength(2)]

    public string Firstname { get; set; }

    [Required]
    [Display(Name = "Last Name")]
    [MinLength(2)]

    public string Lastname { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [Display(Name = "Email")]
    [MinLength(3)]
    [MaxLength(15)]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be 8 characters or longer!")]
    public string Password { get; set; }

    [NotMapped]
    [Compare("Password")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Pw")]
    public string ConfirmPassword { get; set; }
    public int UserAdmin { get; set; }
    public User? CreatedPost { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<Movie> Movies { get; set; } = new List<Movie>();
    public List<MovieTime> MyClubs { get; set; } = new List<MovieTime>();
}


