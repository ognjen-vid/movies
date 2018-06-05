using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class ActorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MovieId { get; set; }
        public string Character { get; set; }
    }
}