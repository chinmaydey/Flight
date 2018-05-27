using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FlightBooking.Controllers
{
    [Route("FlightBooking")]
    public class FlightBookingController : Controller
    {
        private IConfiguration configuration;

        public FlightBookingController(IConfiguration Configuration)
        {
            configuration = Configuration;
        }

        private string ConnectionString
        {
            get { return configuration["ConnectionStrings:FlightBookingDB"]; }
        }

        // GET api/values
        [HttpGet]
        [Route("Flights")]
        public async Task<JsonResult> GetAllFlightsAsync()
        {
            try
            {
                FlightJson flightJsonObj = new FlightJson();
                var result = await flightJsonObj.GetAllFlights(ConnectionString);
                return Json(result);
            }
            catch
            {
                return Json(null);
            }
        }

        [HttpGet]
        [Route("Booking")]
        public async Task<JsonResult> SearchBooking(string fname, string lname, DateTime date, string arrcity, string depcity, string flightno)
        {
            try
            {
                BookingJson bookingJsonObj = new BookingJson();
                var result = await bookingJsonObj.GetBookingsAsync(ConnectionString, fname, lname, date, arrcity, depcity, flightno);
                return Json(result);
            }
            catch
            {
                return Json(null);
            }
        }

        [HttpGet]
        [Route("AvailableFlights")]
        public async Task<JsonResult> GetAvailableFlights(DateTime startdate, DateTime enddate, int passengers)
        {
            try
            {
                FlightJson flightJsonObj = new FlightJson();
                var result = await flightJsonObj.CheckIfAvailableAsync(ConnectionString, startdate, enddate, passengers);
                return Json(result);
            }
            catch
            {
                return Json(null);
            }
        }

        [HttpPost]
        [Route("BookFlight")]
        public async Task<JsonResult> BookFlightAsync(BookingJson bookDetail)
        {
            try
            {
                BookingJson booking = new BookingJson();
                var result = await booking.BookFlightAsync(ConnectionString, bookDetail);
                return Json(result);
            }
            catch
            {
                return Json(null);
            }
        }
    }
}
