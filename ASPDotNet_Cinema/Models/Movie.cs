using ASPDotNet_Cinema.Validation;
using System;
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
        [DecimalPrecision(1, ErrorMessage = "Please round your ranking to 1 significant digit.")]   // controle rankings maar 1 cijfer na de komma (IMDB-stijl).
        [Column(TypeName = "decimal(3,1)")] // Niet strik genomen nodig, maar expliciet datatype om effectief maar 1 significant getal op te slagen
                                            // Opgelet: Dit is DB-afhankelijk (bij Oracle zou dit bijvoorbeeld NUMBER(3,1) moeten zijn)
        [DisplayFormat(DataFormatString = "{0:F1}", ApplyFormatInEditMode = true)]  // ook bij weergave rankings afronden (standaard verschijnen getallen met 2 cijfers na de komma)
                                                                                    // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
                                                                                    // https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.displayformatattribute.dataformatstring?view=netcore-3.1#system-componentmodel-dataannotations-displayformatattribute-dataformatstring)
        public decimal Ranking { get; set; }

        [DisplayFormat(NullDisplayText = "Unknown")]
        public string Director { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Duration { get; set; }

        public string FormattedDuration
        {
            get
            {
                var hours = "hours";
                var minutes = "minutes";
                TimeSpan timeSpan = new TimeSpan(0, Duration, 0);
                if (timeSpan.Hours == 1)
                {
                    hours = hours[..^1];
                }
                if (timeSpan.Minutes == 1)
                {
                    minutes = minutes[..^1];
                }
                return string.Format("{0:%h} {1} {0:%m} {2}", timeSpan, hours, minutes);
            }
        }

        public ICollection<Screening> Screenings { get; set; }  // collection navigation property. Voor het geval dat we alle voorstellingen van een film willen opvragen

    }
}
