using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPDotNet_Cinema.Models
{
    public class Screen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]       // Geen auto increment, we wijzen zelf een zaalnummer toe
        [Range(1, int.MaxValue)]
        public int Number { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }

        public ICollection<Screening> Screenings { get; set; }  // collection navigation property. Voor het geval dat we alle voorstellingen in deze zaal willen opvragen
    }
}
