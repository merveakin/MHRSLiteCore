using MHRSLiteBusinessLayer.Contracts;
using MHRSLiteDataAccessLayer;
using MHRSLiteEntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteBusinessLayer.Implementations
{
    public class ClinicRepository : Repository<Clinic>, IClinicRepository
    {
        private readonly MyContext _myContext;

        public ClinicRepository (MyContext myContext) : base(myContext)
        {
            //Repository'lere kalıtım aldıkları yerdeki metotlar
            //yeterli gözüküyor.Ancak ilerleyen zamanlarda generic yapının karşılamadığı bir ihtiyaç olursa buraya bir metot eklenebilir.O metot _myContext'i kullanarak işlem yapsın diye burada _myContext = myContext yaptık.
            //ÖRN : Bir önceki projemizdeki CategoryRepository'de dashboard için bir ihtiyaç doğmuştu
            //ÖRN : Sistem yöneticilerinin ya da müdürlerin istediği raporlar
            //ÖRN : İstanbuldaki toplam Dahiliye klinik sayısı...
            _myContext = myContext;
        }
    }
}
