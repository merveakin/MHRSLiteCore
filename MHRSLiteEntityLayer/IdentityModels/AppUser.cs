using MHRSLiteEntityLayer.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteEntityLayer.IdentityModels
{
    public class AppUser : IdentityUser
    {
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public string Picture { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        public Genders Gender { get; set; }

    }
}
