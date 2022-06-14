using System.ComponentModel.DataAnnotations;

namespace ASPDotNet_Cinema.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int ScreeningId { get; set; }        // foreign key
        public Screening Screening { get; set; }    // navigation property

        [Required]
        [Range(1, 15)]
        public int Amount { get; set; }
    }
}
