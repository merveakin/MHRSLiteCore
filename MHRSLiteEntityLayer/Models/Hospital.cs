using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteEntityLayer.Models
{
    [Table("Hospitals")]
    public class Hospital :  Base<int>
    {
        [Required]
        [StringLength(400,MinimumLength =2,ErrorMessage ="Hastane adı en az 2 en çok 400 karakter olabilir!")]
        public string HospitalName { get; set; }
        public int DistrictId { get; set; }
        //İlçe tablosuyla ilişki kuruluyor.
        [ForeignKey("DistrictId")]
        public virtual District HospitalDistrict { get; set; }
        //HospitalClinics tablosunda ilişki kuruldu.
        public virtual List<HospitalClinic> HospitalClinics { get; set; }
    }
}
