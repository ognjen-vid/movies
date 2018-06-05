using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI.Interfaces;
using WebAPI.Models;
using System.Data.Entity;

namespace WebAPI.Repository
{
    public class ActorRepository : IDisposable, IActorRepository
    {
        private WebAPIContext db = new WebAPIContext();

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

        public IEnumerable<Actor> GetAll()
        {
            return db.Actors;
        }

        public Actor GetById(int id)
        {
            return db.Actors.FirstOrDefault(a => a.Id == id);
        }

        public void Add(Actor actor)
        {
            db.Actors.Add(actor);
            db.SaveChanges();
        }

        public IEnumerable<Actor> GetByMovieId(int id)
        {
            return db.Actors.Where(a => a.Movie.Id == id);
        }
    }
}