using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASPDotNet_Cinema.Models
{
    public class Screening
    {
        public int Id { get; set; }
        public int ScreenId { get; set; }   // foreign key
        public int MovieId { get; set; }    // foreign key
        public Screen Screen { get; set; }  // navigation property, niet strik noodzakelijk voor EF, maar wel nuttig als programmeur
        public Movie Movie { get; set; }    // navigation property
        
        [Required]
        [DisplayName("Start time")]
        public DateTime StartTime { get; set; }

        [DisplayName("End time")]
        public DateTime EndTime => StartTime.AddMinutes(Movie?.Length ?? 0);
    }
}
