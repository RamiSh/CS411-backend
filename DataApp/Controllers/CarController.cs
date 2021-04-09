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
    public class CarController : ControllerBase
    {
        string connection = "SERVER=127.0.0.1;PORT=3306;UID=root;DATABASE=Main";

        /// <summary>
        /// A demo endpoint that returns the first 5 cars from the Main.CarsDB table.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<Car> Get()
        {
            using var conn = new MySqlConnection(connection);
            var cars = conn.Query<Car>("select * from CarsDB limit 5").ToList();
            return cars;
        }

        /// <summary>
        /// Returns a car from the Main.CarsDB by car id.
        /// </summary>
        /// <param name="id">the car id (Rowid) in the database.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public List<Car> Get(long id)
        {
            using var conn = new MySqlConnection(connection);
            var cars = conn.Query<Car>("select * from CarsDB where RowId = @id", new
            {
                id
            }).ToList();
            return cars;
        }

        /// <summary>
        /// Update an existing car's details in the Main.CarsDB table.
        /// </summary>
        /// <param name="car">The updated car details</param>
        /// <returns>Returns the car after being updated.</returns>
        [HttpPut]
        [Route("")]
        public Car Update([FromBody] Car car)
        {
            using var conn = new MySqlConnection(connection);
            var updatedRows = conn.Execute(@"update Main.CarsDB set
                                                            region = @region,
                                                            price = @price,
                                                            `year` = @year,
                                                            manufacturer = @manufacturer,
                                                            model = @model,
                                                            `condition` = @condition, 
                                                            cylinders = @cylinders, 
                                                            fuel = @fuel, 
                                                            odometer = @odometer, 
                                                            title_status = @title_status, 
                                                            transmission = @transmission, 
                                                            VIN = @VIN, 
                                                            drive = @drive, 
                                                            `size` = @size, 
                                                            `type` = @type, 
                                                            paint_color = @paint_color, 
                                                            state = @state, 
                                                            posting_date = @posting_date 
                                                        where RowId= @RowId", 
                                                        car);

            return car;
        }

        /// <summary>
        /// Adds a new car to the Main.CarsDB table.
        /// </summary>
        /// <param name="car"></param>
        /// <returns>Returns the car with the rowid populated.</returns>
        [HttpPost]
        [Route("")]
        public Car Post(Car car)
        {
            using var conn = new MySqlConnection(connection);
            car.posting_date = DateTime.Now;
            var rowsInserted = conn.Execute(@"INSERT INTO Main.CarsDB
(region, price, `year`, manufacturer, model, `condition`, cylinders, fuel, odometer, title_status, transmission, VIN, drive, `size`, `type`, paint_color, state, posting_date)
VALUES(@region, @price, @year, @manufacturer, @model, @condition, @cylinders, @fuel, @odometer, @title_status, @transmission, @VIN, @drive, @size, @type, @paint_color, @state, @posting_date);
            ", car);

            // get latest inserted car id
            var rowId = conn.QuerySingle<int>("select max(RowId) from Main.CarsDB");
            car.RowId = rowId;
            return car;
        }

        /// <summary>
        /// Deletes a car from the Main.CarsDB by car id.
        /// </summary>
        /// <param name="id">the car id (Rowid) in the database.</param>
        /// <returns>boolean whether the car has been deleted.</returns>
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
