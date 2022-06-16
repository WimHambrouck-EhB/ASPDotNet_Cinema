﻿using ASPDotNet_Cinema.Enums;
using System.Collections.Generic;

namespace ASPDotNet_Cinema.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Screening> Screenings { get; set; }
        public DateRange Range { get; set; }
    }
}
