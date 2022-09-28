# TheMovieTime

This is a social distancing movie club for users to collaborate in a movie club and discuss movies. Feel free to suggest modifications to the club or even add new features!

## Features

<img src="https://api.tvmaze.com/seasons/4/episodes">

I started to build this application with an API for movies, but unable to find it for free, I created a combination between the API where the movies are displayed of the second section and adding manually the movies and saving them in the database.

There is still a lot to improve, but with time, definitely I can do much more.

Users can register or login to TheMovieTime.

<img src="http://localhost:5085/Register" alt="demo login registration">

View your dashboard, add movies, search or view for movies and check out the discussion boards for each specific movie. 

## Example Code

```
[HttpGet("home")]
    public IActionResult Home()
    {
        User current = GetUser();
        if (current == null)
        {
            return Redirect("/");
        }
        ViewBag.User = current;
        List<Movie> Trending = _context.Movies
                                        .Include(b => b.Adder)
                                        .Include(b => b.Members)
                                        .ThenInclude(wp => wp.ClubGoer)
                                        .OrderBy(b => b.CreatedAt)
                                        .Take(2)
                                        .ToList();             
        ViewBag.MyFaves = _context.Users
                            .Include(u => u.MyClubs)
                            .ThenInclude(bc => bc.ThisMovie)
                            .FirstOrDefault(u => u.UserId == current.UserId)
                            .MyClubs.Select(bc => bc.ThisMovie) 
                            .ToList();
        return View("Home", Trending); 
    }
```

### Technologies Used

- C#/.Net
- SQL
- CSS/HTML
- Bootstrap
