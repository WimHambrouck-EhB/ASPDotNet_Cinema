using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ASPDotNet_Cinema.Areas.Identity.Data
{

    public class CinemaUser : IdentityUser
    {
        public const string STAFF_ROLE = "Staff";

    }
}
