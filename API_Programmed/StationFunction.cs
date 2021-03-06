using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using API_Programmed.Models;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Diagnostics;

namespace API_Programmed
{
    public static class StationFunction
    {
        [FunctionName("addGasStations")]
        public static async Task<IActionResult> addGasStations(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "gasStations")] HttpRequest req,
            ILogger log)
        {

            try
            {
                var json = await new StreamReader(req.Body).ReadToEndAsync();
                var registration = JsonConvert.DeserializeObject<Tankstation>(json);

                string connectionString = Environment.GetEnvironmentVariable("SQLSERVER");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = connection;

                        sqlCommand.CommandText = "INSERT INTO Tankstations VALUES(@Coords, @Adres, @StationNaam)";

                        sqlCommand.Parameters.AddWithValue("@Coords", registration.Coords);
                        sqlCommand.Parameters.AddWithValue("@Adres", registration.Adres);
                        sqlCommand.Parameters.AddWithValue("@StationNaam", registration.Naam);


                        await sqlCommand.ExecuteNonQueryAsync();


                    }
                }
                return new OkObjectResult("Het is gelukt!");
            }
            catch (Exception ex)
            {

                log.LogError(ex.ToString());
                return new StatusCodeResult(500);
            }

        }

        [FunctionName("GetGasStations")]
        public static async Task<IActionResult> GetGasStations(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "gasStations")] HttpRequest req,
            ILogger log)
        {

            try
            {
                string connectionString = Environment.GetEnvironmentVariable("SQLSERVER");

                List<Tankstation> tankStations = new List<Tankstation>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = connection;
                        sqlCommand.CommandText = "SELECT Coords, Adres, StationNaam FROM Tankstations";

                        var reader = await sqlCommand.ExecuteReaderAsync();

                        while (await reader.ReadAsync())
                        {
                            tankStations.Add(new Tankstation()
                            {
                                Coords = reader["Coords"].ToString(),
                                Adres = reader["Adres"].ToString(),
                                Naam = reader["StationNaam"].ToString()


                            });

                        }

                    }
                }
                    return new OkObjectResult(tankStations);
            }
            catch (Exception ex)
            {

                log.LogError(ex.ToString());
                return new StatusCodeResult(500);
            }

        }

        [FunctionName("addGasStationsEuro95")]
        public static async Task<IActionResult> addGasStationsEuro95(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "gasStations/gastype")] HttpRequest req,
            ILogger log)
        {

            try
            {
                var json = await new StreamReader(req.Body).ReadToEndAsync();
                var registration = JsonConvert.DeserializeObject<Tankstation>(json);

                string connectionString = Environment.GetEnvironmentVariable("SQLSERVER");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = connection;
                        //opletten in postman is het naam!!!
                        //{
                        //    "coords" : "51.041028, 3.398512",
                        //    "Adres": "Oeselgem",
                        //    "Naam" : "shell",
                        //    "GasType": "euro95",
                        //    "Price": 1.1
                        //}
                        sqlCommand.CommandText = "INSERT INTO Tankstations VALUES(@Coords, @Adres, @StationNaam, @Gastype, @Price)";

                        sqlCommand.Parameters.AddWithValue("@Coords", registration.Coords);
                        sqlCommand.Parameters.AddWithValue("@Adres", registration.Adres);
                        sqlCommand.Parameters.AddWithValue("@StationNaam", registration.Naam);
                        sqlCommand.Parameters.AddWithValue("@Gastype", registration.Gastype);
                        sqlCommand.Parameters.AddWithValue("@Price", registration.Price);



                        await sqlCommand.ExecuteNonQueryAsync();


                    }
                }
                return new OkObjectResult("Het is gelukt!");
            }
            catch (Exception ex)
            {

                log.LogError(ex.ToString());
                return new StatusCodeResult(500);
            }

        }


        [FunctionName("GetGasStationsEuro95")]
        public static async Task<IActionResult> GetGasStationsEuro95(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "gasStations/euro95")] HttpRequest req,
            ILogger log)
        {

            try
            {
                string connectionString = Environment.GetEnvironmentVariable("SQLSERVER");

                List<Tankstation> tankStations = new List<Tankstation>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = connection;
                        sqlCommand.CommandText = "SELECT * FROM Tankstations WHERE gastype = 'euro95'";

                        var reader = await sqlCommand.ExecuteReaderAsync();

                        while (await reader.ReadAsync())
                        {
                            tankStations.Add(new Tankstation()
                            {
                                Coords = reader["Coords"].ToString(),
                                Adres = reader["Adres"].ToString(),
                                Naam = reader["StationNaam"].ToString(),
                                Gastype = reader["GasType"].ToString(),
                                Price = double.Parse(reader["Price"].ToString())
                            }) ;

                        }

                    }
                }
                return new OkObjectResult(tankStations);
            }
            catch (Exception ex)
            {

                log.LogError(ex.ToString());
                return new StatusCodeResult(500);
            }

        }

        [FunctionName("GetGasStationsEuro98")]
        public static async Task<IActionResult> GetGasStationsEuro98(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "gasStations/euro98")] HttpRequest req,
           ILogger log)
        {

            try
            {
                string connectionString = Environment.GetEnvironmentVariable("SQLSERVER");

                List<Tankstation> tankStations = new List<Tankstation>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = connection;
                        sqlCommand.CommandText = "SELECT * FROM Tankstations WHERE gastype = 'euro98'";

                        var reader = await sqlCommand.ExecuteReaderAsync();

                        while (await reader.ReadAsync())
                        {
                            tankStations.Add(new Tankstation()
                            {
                                Coords = reader["Coords"].ToString(),
                                Adres = reader["Adres"].ToString(),
                                Naam = reader["StationNaam"].ToString(),
                                Gastype = reader["GasType"].ToString(),
                                Price = double.Parse(reader["Price"].ToString())
                            });

                        }

                    }
                }
                return new OkObjectResult(tankStations);
            }
            catch (Exception ex)
            {

                log.LogError(ex.ToString());
                return new StatusCodeResult(500);
            }

        }

        [FunctionName("GetGasStationsDiesel")]
        public static async Task<IActionResult> GetGasStationsDiesel(
         [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "gasStations/diesel")] HttpRequest req,
         ILogger log)
        {

            try
            {
                string connectionString = Environment.GetEnvironmentVariable("SQLSERVER");

                List<Tankstation> tankStations = new List<Tankstation>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = connection;
                        sqlCommand.CommandText = "SELECT * FROM Tankstations WHERE gastype = 'diesel'";

                        var reader = await sqlCommand.ExecuteReaderAsync();

                        while (await reader.ReadAsync())
                        {
                            tankStations.Add(new Tankstation()
                            {
                                Coords = reader["Coords"].ToString(),
                                Adres = reader["Adres"].ToString(),
                                Naam = reader["StationNaam"].ToString(),
                                Gastype = reader["GasType"].ToString(),
                                Price = double.Parse(reader["Price"].ToString())
                            });

                        }

                    }
                }
                return new OkObjectResult(tankStations);
            }
            catch (Exception ex)
            {

                log.LogError(ex.ToString());
                return new StatusCodeResult(500);
            }

        }

        [FunctionName("GetAverageGasPrice")]
        public static async Task<IActionResult> GetAverageGasPrice(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "AverageGasPrice/{stationName}")] HttpRequest req,
           string stationName,
           ILogger log)
        {

            try
            {
                string connectionString = Environment.GetEnvironmentVariable("SQLSERVER");

                Average average = new Average();
                

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();


                    using (SqlCommand sqlCommand = new SqlCommand())
                    {

                        sqlCommand.Connection = connection;
                        sqlCommand.CommandText = $"SELECT AVG(Price) as averagePrice FROM Tankstations WHERE StationNaam = '{stationName}'";

                        var reader = await sqlCommand.ExecuteReaderAsync();
                        Debug.WriteLine("hier zijn we dan!");
                        

                        while (await reader.ReadAsync())
                        {
                            Debug.WriteLine(reader["averagePrice"]);
                            average.Avg = (decimal)reader["averagePrice"];
                            //Debug.WriteLine(sum);
                        }


                    }
                }
                //return new OkObjectResult($"Average : {"String.Format("{0:0.##}", sum)}");
                return new OkObjectResult(average);

            }
            catch (Exception ex)
            {

                log.LogError(ex.ToString());
                return new StatusCodeResult(500);
            }

        }



    }
}
