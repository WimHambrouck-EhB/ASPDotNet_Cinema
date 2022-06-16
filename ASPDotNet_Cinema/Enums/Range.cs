using System.ComponentModel.DataAnnotations;

namespace ASPDotNet_Cinema.Enums
{
    public enum DateRange
    {
        Today,
        [Display(Name = "This week")]
        ThisWeek,
        [Display(Name = "Next week")]
        NextWeek
    }
}
