using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightBooking.Models
{
    public static class FlightBookingInitializer
    {
        public static void Initialize(FlightsBookingContext con)
        {
            con.Database.EnsureCreated();

            if (con.Flights.Any())
            {
                return;
            }

            if (con.Bookings.Any())
            {
                return;
            }

            Flight[] flights = new Flight[]
            {
                new Flight()
                {
                     FlightID=1, ArrCity="Melbourne", DepartCity="Sydney", PassCapacity=200,
                    StartTime = new DateTime(2018,05,27, 14,30,00), EndTime= new DateTime(2018,05,27, 15,30,00), FlightNo="QF360"
                }
            };

            foreach (var flight in flights)
            {
                con.Flights.Add(flight);
            }

            con.SaveChanges();

            Booking[] bookings = new Booking[]
            {
                new Booking()
                {
                     BookingID=1,BookDate=new DateTime(2018,05,26, 09,30,00), ArrCity="Melbourne",
                    DepartCity = "Sydney", PassFirstName="John", PassLastName="Doe", FlightNo="QF360"
                },
                new Booking()
                {
                     BookingID=2,BookDate=new DateTime(2018,05,26, 09,30,00), ArrCity="Melbourne",
                    DepartCity = "Sydney", PassFirstName="John", PassLastName="Citizen", FlightNo="QF360"
                }
            };

            foreach (var booking in bookings)
            {
                con.Bookings.Add(booking);
            }

            con.SaveChanges();
        }
    }
}

