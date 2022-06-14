using ASPDotNet_Cinema.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASPDotNet_Cinema.Models
{
    public class Movie
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }

        [Range(0, 10)]
        [DecimalPrecision(1, ErrorMessage = "Please round your ranking to 1 significant digit.")]
        // rankings maar 1 cijfer na de komma (IMDB-stijl). Alternatief hier is [DataType("decimal(3,1)")], maar dit is db-afhankelijk (in Oracle zou het bijvoorbeeld NUMBER(3,1) moeten zijn)
        public decimal Ranking { get; set; }

        private string director;

        public string Director
        {
            get { return director ?? "Unknown"; }
            set { director = value; }
        }

        public ICollection<Screening> Screenings { get; set; }  // collection navigation property. Voor het geval dat we alle voorstellingen van een film willen opvragen

    }
}
