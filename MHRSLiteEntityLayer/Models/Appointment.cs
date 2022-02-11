using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteEntityLayer.Models
{
    public class Appointment : Base<int>
    {
        public string PatientId { get; set; }
        public int HospitalClinicId { get; set; }
        [Required]
        public DateTime AppointmentDate { get; set; }
        [Required]
        [StringLength(5,MinimumLength =5,ErrorMessage ="Randevu saat, XX:XX şeklinde olmalıdır!")]
        public string AppointmentHour { get; set; } //10:00

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
        [ForeignKey("HospitalClinicId")]
        public virtual HospitalClinics HospitalClinic { get; set; }
    }
}
