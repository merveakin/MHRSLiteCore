using MHRSLiteEntityLayer.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteEntityLayer.Models
{
    public class HospitalClinics : Base<int>
    {
        //Hastane ile ilişki kuruldu.
        public int HospitalId { get; set; }
        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }

        //Klinik ile ilişki kuruldu.
        public int ClinicId { get; set; }
        [ForeignKey("ClinicId")]
        public virtual Clinic Clinic { get; set; }

        //Doktor ile ilişki kuruldu.
        public string DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }
         public virtual List<AppointmentHours> AppointmentHours { get; set; }
        public virtual List<Appointment> ClinicAppointments { get; set; }
    }
}
