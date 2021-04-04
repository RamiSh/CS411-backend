using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Dapper;
using DataApp.Models;
using System;

namespace DataApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarController : Controller
    {
        string connection = "SERVER=127.0.0.1;PORT=3306;UID=root;DATABASE=Main";

        [HttpGet]
        public List<Car> Get()
        {
            using var conn = new MySqlConnection(connection);
            var cars = conn.Query<Car>("select * from CarsDB limit 5").ToList();
            return cars;
        }

        [HttpGet]
        [Route("{id}")]
        public List<Car> Get(long id )
        {
            using var conn = new MySqlConnection(connection);
            var cars = conn.Query<Car>("select * from CarsDB where RowId = @id", new
            {
                id
            }).ToList();
            return cars;
        }

        [HttpPost]
        [Route("")]
        public Car Post(Car car)
        {
            using var conn = new MySqlConnection(connection);
            car.posting_date = DateTime.Now;
            var rowsInserted = conn.Execute(@"INSERT INTO Main.CarsDB
(id, region, price, `year`, manufacturer, model, `condition`, cylinders, fuel, odometer, title_status, transmission, VIN, drive, `size`, `type`, paint_color, state, posting_date)
VALUES(null, @region, @price, @year, @manufacturer, @model, @condition, @cylinders, @fuel, @odometer, @title_status, @transmission, @VIN, @drive, @size, @type, @paint_color, @state, @posting_date);
            ", car);

            // get latest inserted car id
            var rowId = conn.QuerySingle<int>("select max(RowId) from Main.CarsDB");
            car.RowId = rowId;
            return car;
        }

        [HttpDelete]
        [Route("{id}")]
        public bool Delete(int id)
        {
            using var conn = new MySqlConnection(connection);
            var rowsDeleted = conn.Execute(@"delete from Main.CarsDB where RowId = @id", new
            {
                id
            });

            return rowsDeleted > 0;
        }
    }
}
