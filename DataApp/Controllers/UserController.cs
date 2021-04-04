using System.Collections.Generic;
using System.Linq;
using Dapper;
using DataApp.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace DataApp.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UserController : ControllerBase
    {
        string connection = "SERVER=127.0.0.1;PORT=3306;UID=root;DATABASE=Main";

        /// <summary>
        /// Returns all users from the Main.Users table.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<User> Get()
        {
            using var conn = new MySqlConnection(connection);
            var users = conn.Query<User>("select * from Main.Users").ToList();
            return users;
        }

        /// <summary>
        /// Returns a user from the Main.Users by user id.
        /// </summary>
        /// <param name="id">the user id (UserId) in the database.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public User Get(int id)
        {
            using var conn = new MySqlConnection(connection);
            var User = conn.QuerySingle<User>("select * from Main.Users where UserId = @id", new
            {
                id
            });
            return User;
        }

        /// <summary>
        /// Adds a new user to the Main.Users table.
        /// </summary>
        /// <param name="car"></param>
        /// <returns>Returns the user with the UserId populated.</returns>
        [HttpPost]
        [Route("")]
        public User Post(User user)
        {
            using var conn = new MySqlConnection(connection);
            var rowsInserted = conn.Execute(@"INSERT INTO Main.Users
(Username, Password, email) VALUES(@Username, @Password, @email);
            ", user);

            // get latest inserted car id
            var rowId = conn.QuerySingle<int>("select max(UserID) from Main.Users");
            user.UserId = rowId;
            return user;
        }

        /// <summary>
        /// Deletes a user from the Main.Users by user id.
        /// </summary>
        /// <param name="id">the user id (UserID) in the database.</param>
        /// <returns>boolean whether the user has been deleted.</returns>
        [HttpDelete]
        [Route("{id}")]
        public bool Delete(int id)
        {
            using var conn = new MySqlConnection(connection);
            var rowsDeleted = conn.Execute(@"delete from Main.Users where UserID = @id", new
            {
                id
            });

            return rowsDeleted > 0;
        }
    }
}
