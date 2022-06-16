using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASPDotNet_Cinema.Models
{
    public class Screening
    {
        private const string DATE_FORMAT = "dd/MM/yyyy HH:mm";
        public int Id { get; set; }
        public int ScreenId { get; set; }   // foreign key
        public int MovieId { get; set; }    // foreign key
        public Screen Screen { get; set; }  // navigation property, niet strik noodzakelijk voor EF, maar wel nuttig als programmeur
        public Movie Movie { get; set; }    // navigation property
        
        [Required]
        [DisplayName("Start time")]
        [DisplayFormat(DataFormatString = "{0:" + DATE_FORMAT + "}")]
        public DateTime StartTime { get; set; }

        [DisplayName("End time")]
        [DisplayFormat(DataFormatString = "{0:" + DATE_FORMAT + "}")]
        public DateTime EndTime => StartTime.AddMinutes(Movie?.Duration ?? 0);
        public string FullName => Movie?.Title + " (" + StartTime.ToString(DATE_FORMAT) + ")";
    }
}
