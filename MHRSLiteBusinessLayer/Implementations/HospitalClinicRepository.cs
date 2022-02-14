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
    public class HospitalClinicRepository : Repository<HospitalClinic>, IHospitalClinicRepository
    {
        private readonly MyContext _myContext;

        public HospitalClinicRepository(MyContext myContext) : base(myContext)
        {
            _myContext = myContext;
        }
    }
}
