using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Photo_WebApp.Models
{
    public class Album 
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        private List<Photo> Photos { get; set; }
    }
}
