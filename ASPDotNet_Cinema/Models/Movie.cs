using ASPDotNet_Cinema.Validation;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPDotNet_Cinema.Models
{
    public class Movie
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }

        [Range(0, 10)]
        [DecimalPrecision(1, ErrorMessage = "Please round your ranking to 1 significant digit.")]   // rankings maar 1 cijfer na de komma (IMDB-stijl).
        [Column(TypeName = "decimal(3,1)")] // Niet strik genomen nodig, maar expliciet datatype om warning van EF te vermijden.
                                            // Opgelet: Dit is DB-afhankelijk (bij Oracle zou dit bijvoorbeeld NUMBER(3,1) moeten zijn)
        public decimal Ranking { get; set; }

        private string director;    // backing field voor Director

        public string Director      // full property voor eigen getter logica (indien geen naam ingevuld, "Unknown" teruggeven)
        {
            get { return director ?? "Unknown"; }
            set { director = value; }
        }

        public ICollection<Screening> Screenings { get; set; }  // collection navigation property. Voor het geval dat we alle voorstellingen van een film willen opvragen

    }
}
