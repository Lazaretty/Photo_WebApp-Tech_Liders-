using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Photo_WebApp.Models
{
    public class PhotoViewModel
    {
        public IFormFile ImageData { get; set; }
        public string Name { get; set; }
        public string CameraName { get; set; }
        public string ShootingParameters { get; set; }
        public string Category { get; set; }
    }
}

