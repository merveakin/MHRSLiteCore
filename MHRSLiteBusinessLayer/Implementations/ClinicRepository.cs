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

        public ClinicRepository(MyContext myContext) : base(myContext)
        {

        }
        public void Deneme()
        {
            //Repository'lere kalıtım aldıkları yerdeki metotlar
            //yeterli gözüküyor.Ancak ilerleyen zamanlarda generic yapının karşılamadığı bir ihtiyaç olursa buraya bir metot eklenebilir.O metot _myContext'i kullanarak işlem yapsın diye burada _myContext ' i protected özelliği ile kalıtım aldık.
            //ÖRN : Bir önceki projemizdeki CategoryRepository'de dashboard için bir ihtiyaç doğmuştu
            //ÖRN : Sistem yöneticilerinin ya da müdürlerin istediği raporlar
            //ÖRN : İstanbuldaki toplam Dahiliye klinik sayısı...
            //Aşağıdaki gibi kullanımlar yapabiliriz.
            //var x = from h in _myContext.Hospitals
        }
    }
}
