using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Genres { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string ReleaseDate { get; set; }
        public string Storyline { get; set; }
        public string Directors { get; set; }


    }
}