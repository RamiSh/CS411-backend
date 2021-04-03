using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Dapper;
namespace DataApp.Controllers
{
    [ApiController]
    [Route("")]
    public class CarController : Controller
    {
        string connection = "SERVER=127.0.0.1;PORT=3306;UID=root;DATABASE=Main";

        [HttpGet]
        public List<dynamic> Get()
        {
            using var conn = new MySqlConnection(connection);
            var cars = conn.Query<dynamic>("select * from CarsDB limit 5").ToList();
            return cars;
        }


    }

    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Vin { get; set; }
        public int Year { get; set; }
    }

    public enum MakeType
    {
        Toyota,
        Honda,
        Audi,
        BMW,
        Cadillac,
        Chevrolet
    }
}
