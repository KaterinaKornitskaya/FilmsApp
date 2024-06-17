using FilmsApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmsApp.Controllers
{
    public class FilmController : Controller
    {
        private readonly FilmContext filmContext;
        public FilmController(FilmContext context)
        {
            filmContext = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Film> films = await Task.Run(() =>  filmContext.Films);
            ViewBag.Films = films;
            return View();
        }
    }
}
