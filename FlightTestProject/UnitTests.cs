using FlightBooking;
using FlightBooking.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace FlightTestProject
{   

    [TestClass]
    public class UnitTests
    {
        string connString = string.Empty;

        public string ConnString
        {
            get
            {
                if (string.IsNullOrEmpty(connString))
                {
                    var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json").Build();

                    connString = config["ConnectionStrings:FlightBookingDB"];
                }

                return connString;
            }
        }

        [TestMethod]
        public async Task TestFlightBookingPass()
        {
            BookingJson booking = new BookingJson();
            var result = await booking.BookFlightAsync(ConnString, new BookingJson()
            {
                ArrCity = "Brisbane",
                DepartCity = "Perth",
                BookDate = new System.DateTime(2018, 5, 28),
                FlightNo = "VA234",
                FirstName = "Roger",
                LastName = "Smith"
            });

            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(Microsoft.EntityFrameworkCore.DbUpdateException), "Depart city is null.")]
        public async Task TestFlightBookingFail()
        {
            BookingJson booking = new BookingJson();
            var result = await booking.BookFlightAsync(ConnString, new BookingJson()
            {
                ArrCity = "Brisbane",
                DepartCity = null,
                BookDate = new DateTime(2018, 05, 21),
                FlightNo = "VA234",
                FirstName = "Roger",
                LastName = "Smith"
            });
        }

        [TestMethod]
        public async Task TestGetBookingPass()
        {
            BookingJson booking = new BookingJson();
            var result = await booking.GetBookingsAsync(ConnString, "John", "Doe", DateTime.MinValue, "Melbourne", "", null);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task TestGetBookingFail()
        {
            BookingJson booking = new BookingJson();
            var result = await booking.GetBookingsAsync(ConnString, "John", "Doe", DateTime.MinValue, "Hobart", "", null);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public async Task TestFlightAvailablePass()
        {
            FlightJson flight = new FlightJson();
            // at the time of testing check if number of passengers booking are right
            var result = await flight.CheckIfAvailableAsync(ConnString, new DateTime(2018, 5, 27), new DateTime(2018, 5, 27), 198);
            Assert.IsTrue(result.Count>0);
        }

        [TestMethod]
        public async Task TestFlightAvailableFail()
        {
            FlightJson flight = new FlightJson();
            // at the time of testing check if number of passengers booking are right
            var result = await flight.CheckIfAvailableAsync(ConnString, new DateTime(2018, 5, 29), new DateTime(2018, 5, 29), 5);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public async Task TestAllFlightsPass()
        {            
            FlightJson flight = new FlightJson();
            var result = await flight.GetAllFlights(ConnString);
            Assert.IsTrue(result.Count > 0);
        }
    }
}
