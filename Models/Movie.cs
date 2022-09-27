#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace SoloProject.Models;
using System.ComponentModel.DataAnnotations.Schema;

    public class Movie
    {
        [Key]
        public int MovieId { get;set; }


        [Required(ErrorMessage="Movie Title is required")]
        [MinLength(2,ErrorMessage="Movie Title must be at least 2 characters")]
        public string Title { get;set; }

        [Required(ErrorMessage="Author is required")]
        [MinLength(2,ErrorMessage="Author must be at least 2 characters")]
        public string Author { get;set; }

        [Required(ErrorMessage="Description is required")]
        [MinLength(10,ErrorMessage="Description must be at least 10 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage="Image Url is required")]
        public string ImgUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // This is the foreign key
        public int UserId { get; set; }

        // An activity can have only one user that adds it.
        
        public User Adder { get; set; }

        // many to many
        public List<MovieTime> Members { get; set; } = new List<MovieTime>();
        public List<Movie> Movies {get; set;} = new List<Movie>();
        public List<Message> MovieConvo { get; set; } = new List<Message>();

    }
