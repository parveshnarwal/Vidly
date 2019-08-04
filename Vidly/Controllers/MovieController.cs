using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using System.Data.Entity;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MovieController : Controller
    {

        private ApplicationDbContext _context;


        public MovieController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        //
        // GET: /Movie/
        public ActionResult Index()
        {
            //var movies = _context.Moives.Include(m => m.Genre).ToList();

            return View();
        }

        public ActionResult Details(int id)
        {
            var movie = _context.Moives.Include(m => m.Genre).Where(m => m.Id == id).FirstOrDefault();

            if (movie == null)

            return HttpNotFound();

            else return View(movie);

        }

        public ActionResult New()
        {
           

            var viewModel = new NewMovieViewModel()
                {
                    Genres = _context.Genres.ToList()
                };


            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            if (!ModelState.IsValid)
            {
               

                var viewModel = new Vidly.ViewModels.NewMovieViewModel(movie)
                {
                    Genres =  _context.Genres.ToList()
                };

                return View("New", viewModel);
            }
            _context.Moives.Add(movie);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var movie = _context.Moives.Single(m => m.Id == id);

            if (movie == null) return HttpNotFound();

            var viewModel = new NewMovieViewModel(movie)
            {
                Genres = _context.Genres.ToList()
            };

            return View(viewModel);

        }

        [HttpPost]
        public ActionResult Save(Movie movie)
        {
            var movieInDb = _context.Moives.SingleOrDefault(m => m.Id == movie.Id);

            movieInDb.Name = movie.Name;
            movieInDb.GenreId = movie.GenreId;
            movieInDb.NumberInStock = movie.NumberInStock;
            movieInDb.ReleaseDate = movie.ReleaseDate;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}