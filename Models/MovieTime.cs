#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace SoloProject.Models;
using System.ComponentModel.DataAnnotations.Schema;


public class MovieTime
{
    [Key]
    public int MovieTimeId { get; set; }

    public int UserId { get; set; }
    public int MovieId { get; set; }

    public User ClubGoer { get; set; }
    public Movie ThisMovie { get; set; }

}
public class GetRootList
{
    public List<Search> Search { get; set; }
}

public class Search
{
    public string imdbID { get; set; }
    public string Title { get; set; }
    public string Year { get; set; }
    public string Poster { get; set; }
}


// Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
public class Image
{
    public string medium { get; set; }
    public string original { get; set; }
}

public class Links
{
    public Self self { get; set; }
}

public class Rating
{
    public double average { get; set; }
}
public class GetRootList2
{
    public List<Root> Search { get; set; }
}

public class Root
{
    public int id { get; set; }
    public string url { get; set; }
    public string name { get; set; }
    public int season { get; set; }
    public int number { get; set; }
    public string type { get; set; }
    public string airdate { get; set; }
    public string airtime { get; set; }
    public DateTime airstamp { get; set; }
    public int runtime { get; set; }
    public Rating rating { get; set; }
    public Image image { get; set; }
    public string summary { get; set; }
    public Links _links { get; set; }
}

public class Self
{
    public string href { get; set; }
}

