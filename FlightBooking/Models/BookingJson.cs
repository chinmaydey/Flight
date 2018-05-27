using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightBooking.Models
{
    public class BookingJson
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BookDate { get; set; }
        public string ArrCity { get; set; }
        public string DepartCity { get; set; }
        public string FlightNo { get; set; }

        public FlightsBookingContext GetContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FlightsBookingContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new FlightsBookingContext(optionsBuilder.Options);
        }

        /// <summary>
        /// Gets bookings based on the condition defined
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="fname"></param>
        /// <param name="lname"></param>
        /// <param name="date"></param>
        /// <param name="arrcity"></param>
        /// <param name="depcity"></param>
        /// <param name="flightno"></param>
        /// <returns></returns>
        public async Task<List<BookingJson>> GetBookingsAsync(string connectionString, string fname, string lname, DateTime date, string arrcity, string depcity, string flightno)
        {
            List<BookingJson> listofBookings = new List<BookingJson>();
            var context = GetContext(connectionString);           

            var result = await context.Bookings.Where(t => string.IsNullOrEmpty(fname) || t.PassFirstName.ToUpper().StartsWith(fname.ToUpper()))
                .Where(t => string.IsNullOrEmpty(lname) || t.PassLastName.ToUpper().StartsWith(lname.ToUpper()))
                .Where(t => date == null || date == DateTime.MinValue || t.BookDate.Date == date.Date)
                .Where(t => string.IsNullOrEmpty(arrcity) || t.ArrCity.ToUpper().Equals(arrcity))
                .Where(t => string.IsNullOrEmpty(depcity) || t.DepartCity.ToUpper().Equals(depcity))
                .Where(t => string.IsNullOrEmpty(flightno) || t.FlightNo.ToUpper().Equals(flightno)).ToListAsync();

            if (result != null)
            {
                foreach (var item in result)
                {
                    BookingJson bookingJsonObj = new BookingJson()
                    {
                        FirstName = item.PassFirstName,
                        LastName = item.PassLastName,
                        BookDate =  item.BookDate,                       
                        DepartCity = item.DepartCity,
                        ArrCity = item.ArrCity,
                        FlightNo = item.FlightNo
                    };

                    listofBookings.Add(bookingJsonObj);
                }
            }

            return listofBookings;
        }

        /// <summary>
        /// Book flight given BookJson parameter supplied
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="bookDetail"></param>
        /// <returns></returns>
        public async Task<bool> BookFlightAsync(string connectionString, BookingJson bookDetail)
        {
            var context = GetContext(connectionString);

            context.Bookings.Add(new Booking()
            {
                BookingID = context.Bookings.Count() + 1,
                ArrCity = bookDetail.ArrCity,
                DepartCity = bookDetail.DepartCity,
                BookDate = bookDetail.BookDate,
                FlightNo = bookDetail.FlightNo,
                PassFirstName = bookDetail.FirstName,
                PassLastName = bookDetail.LastName
            });

            await context.SaveChangesAsync();

            return true;
        }
    }
}
