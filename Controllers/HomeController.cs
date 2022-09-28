using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SoloProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
namespace SoloProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private MyContext _context;
    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }
    private PasswordHasher<User> regHasher = new PasswordHasher<User>();
    private PasswordHasher<LoginUser> logHasher = new PasswordHasher<LoginUser>();

    public User GetUser()
    {
        return _context.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId"));
    }
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        //kam marre me Json api, kam krijuar metoden dhe kerkesen per klientin bashke me pergjigjen, 
        // Ku presim (await) per pergjigjen me ane te metodes asinkron.
        //i gjithe ky proces ruhet me viewbag per t'u bere lidhja me frontend.
        var client = new HttpClient();
        var request1 = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://api.tvmaze.com/seasons/1/episodes"),

        };
        using (var response1 = await client.SendAsync(request1))
        {
            List<Root> root = null;
            response1.EnsureSuccessStatusCode();
            var body1 = await response1.Content.ReadAsStringAsync();
            // Console.WriteLine(body1);
            // string jsonString = JsonSerializer.Serialize(body);
            root = JsonSerializer.Deserialize<List<Root>>(body1);

            ViewBag.AllMovies = root;
        }
        var request2 = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://api.tvmaze.com/seasons/3/episodes"),

        };

        using (var response2 = await client.SendAsync(request2))
        {
            List<Root> root = null;
            response2.EnsureSuccessStatusCode();
            var body2 = await response2.Content.ReadAsStringAsync();
            Console.WriteLine(body2);
            // string jsonString = JsonSerializer.Serialize(body);
            root = JsonSerializer.Deserialize<List<Root>>(body2);

            ViewBag.Season2 = root;
        }

        var request3 = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://api.tvmaze.com/seasons/4/episodes"),

        };

        using (var response3 = await client.SendAsync(request3))
        {
            List<Root> root = null;
            response3.EnsureSuccessStatusCode();
            var body3 = await response3.Content.ReadAsStringAsync();
            Console.WriteLine(body3);
            // string jsonString = JsonSerializer.Serialize(body);
            root = JsonSerializer.Deserialize<List<Root>>(body3);
            ViewBag.Season3 = root;
        }
        return View();
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        return View("Login");
    }

    [HttpPost("login")]
    public IActionResult Login(LoginUser lu)
    {
        if (ModelState.IsValid)
        {
            //kam bere kontrollin e validimit
            User userInDB = _context.Users.FirstOrDefault(u => u.Email == lu.LoginEmail);
            if (userInDB == null)//nese useri eshte null, nxirr errorin
            {
                ModelState.AddModelError("LoginEmail", "Invalid Email or Password");
                return View("Login");
            }
            //verifikojme passwordin e Hash
            var result = logHasher.VerifyHashedPassword(lu, userInDB.Password, lu.LoginPassword);
            if (result == 0)
            {
                ModelState.AddModelError("LoginPassword", "Invalid Email or Password");
                return View("Login");
            }
            //Pra pasi logini eshte i sukseshem, kontrolloj per useradmin qe e kam ne db nese 0 apo 1 pra npm kushtit if
            if (userInDB.UserAdmin == 1)
            {
                //pra nese futet useri nuk hapet admini, nese futet admini, hapet
                HttpContext.Session.SetInt32("userId", userInDB.UserId);
                return Redirect("Adminhome");//me dergo ne faqen adminhome
            }
            else
            {//ne te kundert me co ne faqen home si user
                HttpContext.Session.SetInt32("userId", userInDB.UserId);
                return Redirect("home");
            }

        }
        return View("Login");
    }

    [HttpGet("Register")]
    public IActionResult Register()
    {
        return View("Register");
    }

    [HttpPost("Register")]
    public IActionResult Register(User u)
    {
        if (ModelState.IsValid)
        {
            if (_context.Users.FirstOrDefault(usr => usr.Email == u.Email) != null)
            {
                ModelState.AddModelError("Email", "Email is already in use, try logging in!");
                return View("Register");
            }
            //  //hashohet paswordi para logimit
            // PasswordHasher<User> Hasher = new PasswordHasher<User>();
            // //kjo do te thote hasho paswordin e user duke e kap me user.Password.
            // u.Password = Hasher.HashPassword(u, u.Password);
            // //shton ne database te Users(qe e kemi tek mycontext) shton user nga Modeli User 
            // _context.Users.Add(u);
            // //ruan ndryshimet pasi i shtuam
            // _context.SaveChanges();
            // //vendoset session duke vendosur si kusht User id -> kapet me userId ne thonjza si te modeli, pas presjes.
            // //user.UserId qe kontrollon UserId e e atij qe po logohet pra user qe krijuam te funksioni.
            // HttpContext.Session.SetInt32("userId", u.UserId);
            // //te ridrejton tek indeksi ku duam te logohemi
            // return RedirectToAction("Index");
            string hash = regHasher.HashPassword(u, u.Password);
            u.Password = hash;
            _context.Users.Add(u);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("userId", u.UserId);
            return Redirect("Home");
        }
        return View("Register");
    }


    [HttpGet("home")]
    public IActionResult Home()
    {
        //kam krijuar nje objekt te klases user me emrin current dhe e kam inicializuar me konstruktorin getuser()
        User current = GetUser();
        //kam vendosur kushtin nese objekti current user eshte null te me drg te fq perkatese
        if (current == null)
        {
            return Redirect("/");
        }
        //kemi marre te gjitha te dhenat e current user dhe ia kemi vendosur viewbag.user per t'u bere lidhja me frontendin
        //(shkurt viewbag.user ka marre vleren e current ndaj dhe i kemi =)
        ViewBag.User = current;
        //Ketu filtrojme listen e filmave sipas trending duke perdorur query entityframeworkcore duke perfshire adder, anetaret, ClubGoer
        //duke e renditur sipas kohes, duke marre 2 nga lista pra shfaq vetem 2
        List<Movie> Trending = _context.Movies
                                        .Include(b => b.Adder)
                                        .Include(b => b.Members)
                                        .ThenInclude(wp => wp.ClubGoer)
                                        .OrderBy(b => b.CreatedAt)
                                        .Take(2)
                                        .ToList();
        //ketu kemi marre listen e filmave te preferuar nga userat qe e kane krijuar kete liste preferencash 
        //duke perfshire MyClubs, e ketij filmi ku useri eshte i barabarte me userin current                     
        ViewBag.MyFaves = _context.Users
                            .Include(u => u.MyClubs)
                            .ThenInclude(bc => bc.ThisMovie)
                            .FirstOrDefault(u => u.UserId == current.UserId)
                            .MyClubs.Select(bc => bc.ThisMovie) // selekto kete film pikerisht nga lista e perferencave qe ke krijuar
                            .ToList();
        return View("Home", Trending); // me dergo te faqja home
    }

    [HttpPost("search")]
    public IActionResult Search(string q)
    {
        User current = GetUser(); //kam krijuar serish objektin current me kushtin nqs objekti current user eshte null te me drg te fq perkatese
        if (current == null)
        {
            return Redirect("/");
        }
        ViewBag.User = current;
        //Ketu filtrojme listen e filmave sipas trending duke perdorur query entityframeworkcore duke perfshire adder, anetaret, ClubGoer
        //ku permban titullin, autorin nga lista e filmave....(psh q=batman)
        List<Movie> SearchResults = _context.Movies
                                .Include(b => b.Adder)
                                .Include(b => b.Members)
                                .ThenInclude(wp => wp.ClubGoer)
                                .Where(
                                    b => b.Title.Contains(q) ||
                                    b.Author.Contains(q)
                                )
                                .ToList();
        // ViewBag.AllMovies = SearchResults;
        ViewBag.MyFaves = _context.Users
                            .Include(u => u.MyClubs)
                            .ThenInclude(bc => bc.ThisMovie)
                            .FirstOrDefault(u => u.UserId == current.UserId)
                            .MyClubs.Select(bc => bc.ThisMovie)
                            .ToList();
        return View("Home", SearchResults);
    }

    [HttpGet("adminhome")]
    public IActionResult AdminHome()
    {
        User current = GetUser();
        if (current == null)
        {
            return Redirect("/");
        }
        ViewBag.User = current;
        List<Movie> AllMovies = _context.Movies
                                        .Include(b => b.Adder)
                                        .Include(b => b.Members)
                                        .ThenInclude(wp => wp.ClubGoer)
                                        .OrderBy(b => b.Title)
                                        .ToList();
        return View("AdminHome", AllMovies);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet("movie/{movieId}/{status}")]
    public IActionResult ToggleParty(int movieId, string status)
    {
        User current = GetUser();
        if (current == null)
        {
            return Redirect("/");
        }
        //nqs statusi shtyp butonin add nga home.cshtml shton newclub(movie) eshte i barabarte me userid i tanishem,si dhe me movieid
        if (status == "add")
        {
            MovieTime newClub = new MovieTime();
            newClub.UserId = current.UserId;
            newClub.MovieId = movieId;
            _context.MovieClubs.Add(newClub);//shtoje ne db e movieclub
        }
        else if (status == "remove") // ne te kundert nese shtyp butonin remove largoje si film komplet me user dhe moveid
        {
            MovieTime backout = _context.MovieClubs.FirstOrDefault(w => w.UserId == current.UserId && w.MovieId == movieId);
            _context.MovieClubs.Remove(backout);
        }
        _context.SaveChanges(); // ruaj ndryshimet
        return RedirectToAction("Home"); // me dergo te faqja e home
    }

    [HttpGet("movie/{movieId}")]
    public IActionResult ShowMovie(int movieId)
    {
        User current = GetUser();
        if (current == null)
        {
            return Redirect("/");
        }
        ViewBag.User = current;
        Movie thismovie = _context.Movies
                                .Include(b => b.Members)
                                .ThenInclude(w => w.ClubGoer)
                                .Include(b => b.Adder)
                                .FirstOrDefault(b => b.MovieId == movieId); // kerkon ke db edhe merr te parin film qe ka id si id perkatese e filmit klikova, si parametri larte. 
        ViewBag.Movies = thismovie;
        List<Message> Messages = _context.Messages
                                .Include(m => m.Creator)
                                .Include(m => m.Comments)
                                .ThenInclude(c => c.Writer)
                                .Where(m => m.MovieId == movieId)
                                .OrderByDescending(m => m.CreatedAt)//nga ajo qe eshte krijuar me e fundit (e reja)tek me e vjetra
                                .ToList();
        ViewBag.Messages = Messages;
        List<Comment> Comments = _context.Comments
                                .Include(m => m.Maker)
                                .OrderBy(m => m.CreatedAt)
                                .ToList();
        ViewBag.Comments = Comments;
        return View("ShowMovie");
    }

    [HttpPost("movie/{movieId}/message")]
    public IActionResult Message(Message newMessage, int movieId)
    {
        User current = GetUser();
        if (current == null)
        {
            return Redirect("/");
        }
        ViewBag.User = current;
        Movie thismovie = _context.Movies
                                .Include(b => b.Members)
                                .ThenInclude(w => w.ClubGoer)
                                .Include(b => b.Adder)
                                .FirstOrDefault(b => b.MovieId == movieId);
        ViewBag.Movies = thismovie;
        newMessage.UserId = current.UserId;
        newMessage.MovieId = movieId;
        _context.Messages.Add(newMessage);
        if (!(newMessage.Content == null))
        {
            _context.SaveChanges();
            List<Message> Messages = _context.Messages
                        .Include(m => m.Creator)
                        .Include(m => m.Comments)
                        .ThenInclude(c => c.Writer)
                        .OrderBy(m => m.CreatedAt)
                        .ToList();
            ViewBag.Messages = Messages;

        }
        return Redirect($"/movie/{movieId}");
    }

    [HttpGet("movie/{movieId}/message/{messageId}/delete")]
    public IActionResult DeleteMessage(int messageId, int movieId)
    {
        User current = GetUser();
        if (current == null)
        {
            return Redirect("/");
        }
        ViewBag.User = current;
        Movie thismovie = _context.Movies
                                .Include(b => b.Members)
                                .ThenInclude(w => w.ClubGoer)
                                .Include(b => b.Adder)
                                .FirstOrDefault(b => b.MovieId == movieId);
        ViewBag.Movies = thismovie;
        Message todelete = _context.Messages
                        .FirstOrDefault(m => m.MessageId == messageId);
        _context.Messages.Remove(todelete);
        _context.SaveChanges();
        return Redirect($"/movie/{movieId}");
    }

    [HttpPost("movie/{movieId}/{messageId}/comment")]
    public IActionResult Comment(Comment newComment, int commentId, int messageId, int movieId)
    {
        User current = GetUser();
        // if (current == null)
        // {
        //     return Redirect("/");
        // }
        ViewBag.User = current;
        newComment.UserId = current.UserId;
        _context.Comments.Add(newComment);
        _context.SaveChanges();
        List<Comment> Comments = _context.Comments
                                .Include(m => m.Maker)
                                .OrderBy(m => m.CreatedAt)
                                .ToList();
        ViewBag.Comments = Comments;
        Movie thismovie = _context.Movies
                                .Include(b => b.Members)
                                .ThenInclude(w => w.ClubGoer)
                                .Include(b => b.Adder)
                                .FirstOrDefault(b => b.MovieId == movieId);
        ViewBag.Movies = thismovie;
        return Redirect($"/movie/{movieId}");
    }

    [HttpGet("movie/{movieId}/comment/{commentId}/delete")]
    public IActionResult DeleteComment(int commentId, int movieId)
    {
        User current = GetUser();
        if (current == null)
        {
            return Redirect("/");
        }
        ViewBag.User = current;
        Movie thismovie = _context.Movies
                                .Include(b => b.Members)
                                .ThenInclude(w => w.ClubGoer)
                                .Include(b => b.Adder)
                                .FirstOrDefault(b => b.MovieId == movieId);
        ViewBag.Movies = thismovie;
        Comment todelete = _context.Comments
                            .FirstOrDefault(m => m.CommentId == commentId);
        _context.Comments.Remove(todelete);
        _context.SaveChanges();
        return Redirect($"/movie/{movieId}");
    }

    [HttpGet("movies/all")]
    public IActionResult ShowAll(string q)
    {
        User current = GetUser();
        if (current == null)
        {
            return Redirect("/");
        }
        ViewBag.User = current;
        List<Movie> AllMovies = new List<Movie>();
        if (q != null)
        {
            AllMovies = _context.Movies
                            .Include(b => b.Adder)
                            .Include(b => b.Members)
                            .ThenInclude(wp => wp.ClubGoer)
                            .Where(
                                    b => b.Title.Contains(q) ||
                                    b.Author.Contains(q)
                            )
                            .OrderBy(b => b.Title)
                            .ToList();
        }
        else
        {
            AllMovies = _context.Movies
                            .Include(b => b.Adder)
                            .Include(b => b.Members)
                            .ThenInclude(wp => wp.ClubGoer)
                            .OrderBy(b => b.Title)
                            .ToList();
        }
        return View(AllMovies);
    }

    [HttpGet("NewMovie")]
    public IActionResult NewMovie()
    {
        User current = GetUser();
        if (current == null)
        {
            return Redirect("/");
        }
        return View("NewMovie");
    }

    [HttpPost("AddNewMovie")]
    public IActionResult AddMovie(Movie newmovie)
    {
        User current = GetUser();
        // if (current == null)
        // {
        //     return Redirect("/");
        // }
        // if (ModelState.IsValid)
        {
            newmovie.UserId = current.UserId;
            _context.Movies.Add(newmovie);
            _context.SaveChanges();
            return RedirectToAction("Home");
        }

        return View("NewMovie");
    }

    [HttpGet("movie/{movieId}/delete")]
    public IActionResult DeleteMovie(int movieId)
    {
        User current = GetUser();
        if (current == null)
        {
            return Redirect("/");
        }
        Movie remove = _context.Movies.FirstOrDefault(b => b.MovieId == movieId);
        _context.Movies.Remove(remove);
        _context.SaveChanges();
        return RedirectToAction("AdminHome");
    }


    [HttpGet("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("login");
    }
}
