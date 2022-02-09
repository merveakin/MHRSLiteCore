using MHRSLiteEntityLayer.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteEntityLayer.IdentityModels
{
    public class AppUser : IdentityUser
    {
        public string Picture { get; set; }
        public DateTime? BirthDate { get; set; }
        public Genders Gender { get; set; }

    }
}
