using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SoloProject.Models;

public class Message
{
    [Key]
    public int MessageId { get; set; }

    [MinLength(2)]
    [Required(ErrorMessage = "Message is required")]

    public string? Content { get; set; }

    public int UserId { get; set; }

    public User? Creator { get; set; }
    public int MovieId { get; set; }

    [Required(ErrorMessage = "Message is required")]
    public Movie? MovieMessage { get; set; }

    [Required(ErrorMessage = "Message is required")]
    public List<Comment>? Comments { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
