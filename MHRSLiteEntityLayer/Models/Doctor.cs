﻿using MHRSLiteEntityLayer.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteEntityLayer.Models
{
    [Table("Doctors")]
    public class Doctor : PersonBase
    {
        public string UserId { get; set; }// Identity Model'in ID değeri burada ForeignKey olacaktır.
        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }

        public virtual List<HospitalClinics> HospitalClinics { get; set; }
    }
}