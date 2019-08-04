using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using Vidly.Models;
using Vidly.Dtos;
using AutoMapper;
using System.Data.Entity;

namespace Vidly.Controllers.Api
{
    public class MovieController : ApiController
    {
        ApplicationDbContext _context;

        public MovieController()
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/movies

        [HttpGet]
        public IEnumerable<MovieDto> GetMovies()
        {
            return _context.Moives
                .Include(m => m.Genre)
                .ToList()
                .Select(Mapper.Map<Movie, MovieDto>);
        }

        [HttpGet]
        public IHttpActionResult GetMovie(int id)
        {
            var movie = _context.Moives.FirstOrDefault(m => m.Id == id);

            if (movie == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return Ok(Mapper.Map<Movie, MovieDto>(movie));

        }

        [HttpPost]
        public IHttpActionResult CreateMovie(MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var movie = Mapper.Map<MovieDto, Movie>(movieDto); 

            _context.Moives.Add(movie);

            _context.SaveChanges();

            movieDto.Id = movie.Id;

            return Created(new Uri(Request.RequestUri + "/" + movieDto.Id), movieDto);
        }

        [HttpPut]
        public void UpdateMovie(int id, MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var movieInDb = _context.Moives.SingleOrDefault(m => m.Id == id);

            if(movieInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Mapper.Map(movieDto, movieInDb);

            
            _context.SaveChanges();


        }

        [HttpDelete]
        public void DeleteMovie(int id)
        {
            var movie = _context.Moives.FirstOrDefault(m => m.Id == id);

            if (movie == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.Moives.Remove(movie);

            _context.SaveChanges();
        }
    }
}