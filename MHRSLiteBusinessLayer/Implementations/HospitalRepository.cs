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
    public class HospitalRepository : Repository<Hospital>,IHospitalRepository
    {
        private readonly MyContext _myContext;

        public HospitalRepository(MyContext myContext) : base(myContext)
        {
            _myContext = myContext;
        }
    }
}
