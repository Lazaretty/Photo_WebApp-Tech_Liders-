using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Photo_WebApp.Models
{
    public class Photo 
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string CameraName { get; set; }
        public string ShootingParameters { get; set; }
        public string Category { get; set; }
    }
}
