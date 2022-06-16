using System.ComponentModel.DataAnnotations;

namespace ASPDotNet_Cinema.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int ScreeningId { get; set; }        // foreign key
        public Screening Screening { get; set; }    // navigation property

        [Required]
        [Range(1, 15, ErrorMessage = "Please enter an amount between {1} and {2}.<br/>If you want to order more than {2} tickets, send us an e-mail.")]
        public int Amount { get; set; }
    }
}
