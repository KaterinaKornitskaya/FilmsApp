using FilmsApp.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmsApp.Controllers
{
    public class FilmController : Controller
    {
        private readonly FilmContext filmContext;
        private IWebHostEnvironment filmAppEnvironment;
        public FilmController(FilmContext context, IWebHostEnvironment appEnvironment)
        {
            filmContext = context;
            filmAppEnvironment = appEnvironment;
        }

        // список всех фильмов
        public async Task<IActionResult> Index()
        {
            IEnumerable<Film> films = await Task.Run(() =>  filmContext.Films);
           // ViewBag.Films = films;
            return View(films);
        }

        // детали
        public async Task<IActionResult> Details(int? id)
        {
            if(id==null)
                return NotFound();

            var film = await filmContext.Films.FirstOrDefaultAsync(e=>e.ID==id);

            if(film==null)
                return NotFound();
            else
                return  View(film);
        }

        // удаление
        public async Task<IActionResult> Delete (int? id)
        {
            if (id==null)
                return NotFound();
            var film = await filmContext.Films.FirstOrDefaultAsync(e => e.ID == id);

            if(film==null)
                return NotFound();
            return View(film);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id==null)
                return NotFound();
            var film = await filmContext.Films.FirstOrDefaultAsync(e => e.ID == id);

            if( film==null)
                return NotFound();

            filmContext.Films.Remove(film);
            await filmContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // редактирование
        public async Task<IActionResult> Edit (int? id)
        {
            if (id==null)
                return NotFound();
            var film = await filmContext.Films.FirstOrDefaultAsync(e => e.ID==id);
            if(film==null)
                return NotFound();
            return View(film);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int? id, 
            [Bind("ID, Name, Year, Director, Actors, Genre, Description, Image")] Film film,
            IFormFile? uploadedFile)
        {
            if (id != film.ID)
                return NotFound();
          
            if (ModelState.IsValid)
            {
                if (uploadedFile != null)
                {
                    string path = "/img/" + uploadedFile.FileName;
                    using (FileStream fileStream = new FileStream
                        (filmAppEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                        film.Image = path;
                    }
                }
                try
                {
                    filmContext.Update(film);
                    await filmContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!filmContext.Films.Any(e=>e.ID==id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ID, Name, Year, Director, Actors, Genre, Description, Image")] Film film,
            IFormFile? uploadedFile)
        {
            if (SameNameandDirectorExists(film.Name, film.Director))
                ModelState.AddModelError("", "Комбинация такого названия и режиссера уже есть!" +
                    " Введите другое название, или другого режиссера.");
            //if (film.Name == "film")
            //    ModelState.AddModelError("", "Не може бути назва 'film'!");

            if (ModelState.IsValid)
            {
                if (uploadedFile != null)
                {
                    string path = "/img/" + uploadedFile.FileName;
                    using (FileStream fileStream = new FileStream
                        (filmAppEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                        film.Image = path;
                    }
                }
                try
                {
                    filmContext.Add(film);
                    await filmContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

            }
            return View(film);
        }

        // проверка, что существует фильм с такой комбинацией имени и режиссера, для Create
        private bool SameNameandDirectorExists(string name, string director)
        {
            return filmContext.Films.Any(e => e.Name == name && e.Director == director);
        }
    }
}
