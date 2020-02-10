using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;
using Photo_WebApp.Models;
using System.Data;

namespace Photo_WebApp.Repository
{
    public class DB_Context
    {
        string connectionString = null;

        public DB_Context(string conn)
        {
            connectionString = conn;
        }

        public void EditPhoto(Photo photo)
        {
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("AuthorId", photo.AuthorId);
            parameter.Add("CameraName", photo.CameraName);
            parameter.Add("Category", photo.Category);
            parameter.Add("ImageData", photo.ImageData);
            parameter.Add("Name", photo.Name);
            parameter.Add("ShootingParameters", photo.ShootingParameters);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Execute("AddPhoto", parameter, commandType: CommandType.StoredProcedure);
            }
        }

        public void AddAlbum(Album album)
        {
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("UserId", album.UserId);
            parameter.Add("Name", album.Name);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Execute("AddAlbum", parameter, commandType: CommandType.StoredProcedure);
            }
        }

        public List<Photo> GetPhotos()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Photo>("GetAllPhotos", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public List<Album> GetAllAlbums()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Album>("GetAllAlbums", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public List<Photo> GetPhotosFromAlbum(int id)
        {
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("AlbumId", id);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Photo>("GetPhotosFromAlbum", parameter, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public Album GetAlbumById(int id)
        {
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("AlbumId", id);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Album>("GetAlbumById", parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }


        public Album AddPhotoToAlmum(int AlbumId, int PhotoId)
        {
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("AlbumId", AlbumId);
            parameter.Add("PhotoId", PhotoId);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Album>("AddPhotoToAlmum", parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public bool IsRegisted(string Email)
        {
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("EM", Email);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<User>("IsRegisted", parameter, commandType: CommandType.StoredProcedure).FirstOrDefault() != null;
            }
        }

        public void AddUser(User user)
        {
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("Email", user.Email);
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            parameter.Add("Password", user.Password);
            parameter.Add("Nickname", user.Nickname);
       
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Execute("AddUser", parameter, commandType: CommandType.StoredProcedure);
            }

        }

        public bool Login(Login login) // Возвращает TRUE, если пользователь с такой комбинауий e-mail и пароль сущетсвует в БАЗЕ
        {
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("EM", login.Email);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var User = db.Query<User>("IsRegisted", parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (User == null)
                {
                    return false;
                }
                else
                {
                    if (BCrypt.Net.BCrypt.Verify(login.Password, User.Password))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public User GetUser(Login login)
        {
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("EM", login.Email);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<User>("IsRegisted", parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }

        }

        public List<Photo> GetUserPhotos(int UserId)
        {
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("UserId", UserId);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Photo>("GetUsersPhotos", parameter, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public List<Photo> SearchPhoto(string SerachName)
        {
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("SerachName", SerachName);
            using (IDbConnection db = new SqlConnection(connectionString))
             {
                return db.Query<Photo>("SearchPhotos", parameter, commandType: CommandType.StoredProcedure).ToList();
             }
          
        }


        public List<Album> SearchAlbum(string SerachName)
        {
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("SerachName", SerachName);
            using (IDbConnection db = new SqlConnection(connectionString))
            {
               return db.Query<Album>("SearchAlbums", parameter, commandType: CommandType.StoredProcedure).ToList();
            }
            
        }
    }
}
