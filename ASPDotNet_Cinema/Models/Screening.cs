using System;
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
        public DateTime StartTime { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Length { get; set; }
        public DateTime EndTime => StartTime.AddMinutes(Length);

    }
}
