using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightBooking.Models
{
    public class FlightsBookingContext : DbContext
    {
        public FlightsBookingContext(DbContextOptions<FlightsBookingContext> options)
            : base(options)
        {
        }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }

    public class Flight
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FlightID { get; set; }
        [Required]
        [StringLength(50)]
        public string FlightNo { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        [Required]
        [Range(0, 1000)]
        public int PassCapacity { get; set; }
        [Required]
        [StringLength(100)]
        public string DepartCity { get; set; }
        [Required]
        [StringLength(100)]
        public string ArrCity { get; set; }
        public List<Booking> Bookings { get; set; }
    }

    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BookingID { get; set; }
        [Required]
        [StringLength(100)]
        public string PassFirstName { get; set; }
        [Required]
        [StringLength(100)]
        public string PassLastName { get; set; }
        [Required]
        public DateTime BookDate { get; set; }
        [Required]
        [StringLength(100)]
        public string ArrCity { get; set; }
        [Required]
        [StringLength(100)]
        public string DepartCity { get; set; }
        [Required]
        [StringLength(50)]
        public string FlightNo { get; set; }        
    }
}



