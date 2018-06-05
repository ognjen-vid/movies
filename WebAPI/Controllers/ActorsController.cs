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
    public class ActorsController : ApiController
    {

        IActorRepository _actorRepo { get; set; }

        public ActorsController(IActorRepository actorRepo)
        {
            _actorRepo = actorRepo;
        }

        // GET: api/Actors
        public IHttpActionResult GetActors()
        {
            var actors = _actorRepo.GetAll().AsQueryable().ProjectTo<ActorDTO>();
            return Ok(actors);
        }

        // GET: api/Actors/5
        public IHttpActionResult GetActor(int id)
        {
            Actor actor = _actorRepo.GetById(id);
            if (actor == null)
            {
                return NotFound();
            }

            var actorDTO = Mapper.Map<ActorDTO>(actor);
            return Ok(actor);
        }

        // GET: api/Actors/MovieId/5
        [Route("actors/MovieId/{movieId}")]
        [HttpGet]
        public IHttpActionResult GetActorsByMovieId(int movieId)
        {
            var actors = _actorRepo.GetByMovieId(movieId).AsQueryable().ProjectTo<ActorDTO>();
            return Ok(actors);
        }


        // PUT: api/Actors/5
        public IHttpActionResult PutActor(int id, Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != actor.Id)
            {
                return BadRequest();
            }

            _actorRepo.Edit(actor);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Actors
        public IHttpActionResult PostActor(Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _actorRepo.Add(actor);

            return CreatedAtRoute("DefaultApi", new { id = actor.Id }, actor);
        }

        // DELETE: api/Actors/5
        public IHttpActionResult DeleteActor(int id)
        {
            Actor actor = _actorRepo.GetById(id);
            if (actor == null)
            {
                return NotFound();
            }

            _actorRepo.Delete(actor);

            return Ok(actor);
        }
    }
}