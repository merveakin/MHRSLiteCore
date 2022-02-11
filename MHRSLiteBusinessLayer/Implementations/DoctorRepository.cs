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
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {

        private readonly MyContext _myContext;
        public DoctorRepository(MyContext myContext) : base(myContext)
        {

        }
    }
}
