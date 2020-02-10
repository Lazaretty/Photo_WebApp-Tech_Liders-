using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Photo_WebApp.Models;
using System.Web;
using System.IO;
using Photo_WebApp.Repository;
using Microsoft.AspNetCore.Http;

namespace Photo_WebApp.Controllers
{
    public class HomeController : Controller
    {
        DB_Context repos;

        public HomeController(DB_Context r)
        {
            repos = r;
        }

        public IActionResult Index()
        {
            var Albums = repos.GetAllAlbums();
            return View(Albums);
        }


        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(PhotoViewModel photoVm)
        {

            Photo photo = new Photo {
                Name = photoVm.Name,
                CameraName = photoVm.CameraName,
                ShootingParameters = photoVm.ShootingParameters,
                Category = photoVm.Category,
                AuthorId = Int32.Parse(HttpContext.Session.GetString("UserId"))
            };
            if (photoVm.ImageData != null)
            {
                byte[] Image = null;
                using (var binaryReader = new BinaryReader(photoVm.ImageData.OpenReadStream()))
                {
                    Image = binaryReader.ReadBytes((int)photoVm.ImageData.Length);
                }
                photo.ImageData = Image;
            }
            repos.EditPhoto(photo);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddAlbum()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddAlbum(Album album)
        {
            album.UserId = Int32.Parse(HttpContext.Session.GetString("UserId"));
            repos.AddAlbum(album);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult EditAlbum(int id)
        {
            ViewBag.Id = id;
            ViewBag.PhotosInAlbum = repos.GetPhotosFromAlbum(id);
            var Photos = repos.GetUserPhotos(Int32.Parse(HttpContext.Session.GetString("UserId")));
            ViewBag.Name = repos.GetAlbumById(id).Name;
            return View(Photos);
        }

        public void AddPhotoToAlmum(int AlbumId, int PhotoId)
        {
            repos.AddPhotoToAlmum(AlbumId, PhotoId);
        }


        [HttpGet]
        public IActionResult ViewAlbun(int id)
        {
            var photos = repos.GetPhotosFromAlbum(id);
            return View(photos);
        }

        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(string SerachName, int SearchParam)
        {

            if (SerachName != null)  
            {
                if (SearchParam == 1)
                {
                    ViewBag.Result = repos.SearchPhoto(SerachName);
                    ViewBag.Type = "1";
                }
                else
                {
                    ViewBag.Result = repos.SearchAlbum(SerachName);
                    ViewBag.Type = "2";
                }
            }
            return View();
        }
        
    }
}
