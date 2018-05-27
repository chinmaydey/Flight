using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightBooking.Models
{
    public class FlightJson
    {
        public string FlightNo { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int PassCapacity { get; set; }
        public string DepartCity { get; set; }
        public string ArrCity { get; set; }

        public FlightsBookingContext GetContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FlightsBookingContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new FlightsBookingContext(optionsBuilder.Options);
        }

        /// <summary>
        /// Get All flights available
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public async Task<List<FlightJson>> GetAllFlights(string connectionString)
        {
            List<FlightJson> listofFlights = new List<FlightJson>();
            var context = GetContext(connectionString);
            var result = await context.Flights.ToListAsync();

            if (result != null)
            {
                foreach (var item in result)
                {
                    FlightJson flightJsonObj = new FlightJson()
                    {
                        FlightNo = item.FlightNo,
                        StartTime = item.StartTime.ToString("dd-MMM-yyyy hh:mm tt"),
                        EndTime = item.EndTime.ToString("dd-MMM-yyyy hh:mm tt"),
                        PassCapacity = item.PassCapacity,
                        DepartCity = item.DepartCity,
                        ArrCity = item.ArrCity
                    };

                    listofFlights.Add(flightJsonObj);
                }
            }

            return listofFlights;
        }

        /// <summary>
        /// Check if any flight is available for booking, cross check with Booking if the seats are not available
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="passengers"></param>
        /// <returns></returns>
        public async Task<List<FlightJson>> CheckIfAvailableAsync(string connectionString, DateTime startdate, DateTime enddate, int passengers)
        {
            List<FlightJson> listofFlights = new List<FlightJson>();
            var context = GetContext(connectionString);
            var result = await context.Flights.Where(t => t.StartTime.Date >= startdate.Date &&
            t.EndTime.Date <= enddate.Date && passengers <= t.PassCapacity).ToListAsync();

            // flights which are available
            if (result != null)
            {
                foreach (var item in result)
                {
                    // check if the flight is booked out
                    var totalBookings = await context.Bookings.Where(t => t.FlightNo == item.FlightNo).CountAsync();

                    // only flights available which have sufficient number of seats to accomodate current booking
                    if (item.PassCapacity >= totalBookings + passengers)
                    {
                        FlightJson flightJsonObj = new FlightJson()
                        {
                            FlightNo = item.FlightNo,
                            StartTime = item.StartTime.ToString("dd-MMM-yyyy hh:mm tt"),
                            EndTime = item.EndTime.ToString("dd-MMM-yyyy hh:mm tt"),
                            PassCapacity = item.PassCapacity,
                            DepartCity = item.DepartCity,
                            ArrCity = item.ArrCity
                        };

                        listofFlights.Add(flightJsonObj);
                    }
                }
            }

            return listofFlights;
        }        
    }
}
