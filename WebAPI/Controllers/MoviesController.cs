using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using WebAPI.Interfaces;
using PagedList;
using Microsoft.Ajax.Utilities;

namespace WebAPI.Controllers
{
    public class MoviesController : ApiController
    {
        IMovieRepository _movieRepo { get; set; }

        public MoviesController(IMovieRepository movieRepo)
        {
            _movieRepo = movieRepo;
        }

        // GET: api/Movies
        [HttpGet]
        public IHttpActionResult GetMovies([FromUri] SearchData searchData, int pageNum) // Inicijalno trazimo 0 stranu, predefisnisanu u main.js 
        {
            var resultsPerPage = 2;

            var movies = _movieRepo.GetAll()
                .Where(b => b.Title.ToUpper().Contains(searchData.movieName.ToUpper()))
                .AsQueryable()
                .ProjectTo<MovieDTO>();

            var totalMovies = movies.Count(); 
            
            movies = movies.Skip(pageNum * resultsPerPage).Take(resultsPerPage); 
            
            var totalPages = Math.Ceiling((double)totalMovies / resultsPerPage);

            var result = new { TotalMovies = totalMovies, Movies = movies, TotalPages = totalPages };
            return Ok(result); //vraca int, IQueriable, double
        }

        // GET: api/Movies/5
        public IHttpActionResult GetMovie(int Id)
        {
            Movie movie = _movieRepo.GetById(Id);
            if (movie == null)
            {
                return NotFound();
            }
            var movieDto = Mapper.Map<MovieDTO>(movie);
            return Ok(movieDto);
        }

        // PUT: api/Movies/5
        public IHttpActionResult PutMovie(int Id, Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (Id != movie.Id)
            {
                return BadRequest();
            }

            _movieRepo.Edit(movie);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Movies
        public IHttpActionResult PostMovie(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Movie movie = Mapper.Map<Movie>(dto);

            _movieRepo.Add(movie);

            return CreatedAtRoute("DefaultApi", new { id = movie.Id }, movie);
        }

        // DELETE: api/Movies/5
        public IHttpActionResult DeleteMovie(int id)
        {
            Movie movie = _movieRepo.GetById(id);
            if (movie == null)
            {
                return NotFound();
            }

            _movieRepo.Delete(movie);

            return Ok(movie);
        }

    }
}