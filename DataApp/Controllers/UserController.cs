using System;
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
(Username, Password, email) VALUES(@Username, @Password, @email);", user);

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

        /// <summary>
        /// Authenticats a user using its username and password
        /// </summary>
        /// <param name="request">a request that contains the username and password</param>
        /// <returns>boolean whether the user has been authenticated.</returns>
        [HttpPost]
        [Route("authenticate")]
        public bool Authenticate([FromBody] UserAuthenticationRequest request)
        {
            using var conn = new MySqlConnection(connection);
            var userExists = conn.QuerySingleOrDefault<User>("select * from Main.Users where UserName = @UserName and Password = @Password",
                request);
            return userExists != null ? true : false;
        }


        /// <summary>
        /// Links a car to a user. This is a transactional process, if any part of the workflow fails the
        /// database transaction will rollback.
        /// </summary>
        /// <param name="car">car's details</param>
        /// <param name="id">user id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/owns")]
        public IActionResult Owns([FromBody] Car car, int id)
        {
            using var conn = new MySqlConnection(connection);
            conn.Open();
            var trnx = conn.BeginTransaction();

            try
            {
                var carInserted = conn.Execute(Car.AsInsertQuery(), car);
                var rowId = conn.QuerySingle<int>("select max(RowId) from Main.CarsDB");

                var ownsInserted = conn.Execute(
                    @"insert into Main.Owns 
                            (UserID, CarID, PriceBought, DateBought)
                            VALUES(@id, @carId, @priceBought, @dateBought)",
                new
                {
                    id = id,
                    carId = rowId,
                    priceBought = car.price,
                    dateBought = car.posting_date
                });
                trnx.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                trnx.Rollback();
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Retrieves all cars that owned by a user.
        /// </summary>
        /// <param name="id">The user id to get cars for</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/cars")]
        public IEnumerable<Car> Cars(int id)
        {
            using var conn = new MySqlConnection(connection);
            var cars = conn.Query<Car>(@"select c.*
                                            from Main.CarsDB c
                                            inner join Main.Owns o on c.RowId = o.CarID 
                                            where o.userID = @userId",
                param: new
                {
                    userId = id
                });

            return cars;
        }
    }
}
