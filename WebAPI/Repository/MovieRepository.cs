using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI.Interfaces;
using WebAPI.Models;
using System.Data.Entity;

namespace WebAPI.Repository
{
    public class MovieRepository : IDisposable, IMovieRepository
    {
        private WebAPIContext db = new WebAPIContext();

        public void Add(Movie movie)
        {
            db.Movies.Add(movie);
            db.SaveChanges();
        }

        public void Edit(Movie movie)
        {
            db.Entry(movie).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(Movie movie)
        {
            db.Movies.Remove(movie);
            db.SaveChanges();
        }

        public IEnumerable<Movie> GetAll()
        {
            return db.Movies.Include(m => m.Actors);
        }

        public Movie GetById(int id)
        {
            return db.Movies.Include(b => b.Actors).FirstOrDefault(b => b.Id == id);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}