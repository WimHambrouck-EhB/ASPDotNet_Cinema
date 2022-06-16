using System.ComponentModel.DataAnnotations;

namespace ASPDotNet_Cinema.Models.ViewModels
{
    public class ScreeningWithSum
    {
        public Screening Screening{ get; set; }

        [Display(Name = "Seats")]
        public int TicketsLeft { get; set; }
    }
}
