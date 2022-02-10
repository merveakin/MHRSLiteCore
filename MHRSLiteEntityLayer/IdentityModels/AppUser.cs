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

        [StringLength(50,MinimumLength =2,ErrorMessage ="İsminiz en az 2 en çok 50 karakter olmalıdır!")]
        [Required(ErrorMessage = "İsim gereklidir.")]
        public string Name { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Soyisminiz en az 2 en çok 50 karakter olmalıdır!")]
        [Required(ErrorMessage = "Soyisim gereklidir.")]
        public string Surname { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public string Picture { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        [Required(ErrorMessage = "Cinsiyet bilgisi gereklidir.")]
        public Genders Gender { get; set; }

    }
}
