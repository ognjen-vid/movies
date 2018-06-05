using Unity;
using WebAPI.Interfaces;
using WebAPI.Repository;
using WebAPI.Resolver;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // AutoMapper
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Movie, MovieDTO>()
                    .ForMember(dto => dto.ReleaseDate, opt => opt.MapFrom(movie => movie.ReleaseDate.ToString()));
                cfg.CreateMap<MovieDTO, Movie>();
                cfg.CreateMap<Actor, ActorDTO>();
                cfg.CreateMap<ActorDTO, Actor>();
            });

            // Unity
            var container = new UnityContainer();
            container.RegisterType<IMovieRepository, MovieRepository>(new Unity.Lifetime.HierarchicalLifetimeManager());
            container.RegisterType<IActorRepository, ActorRepository>(new Unity.Lifetime.HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);
        }
    }
}
