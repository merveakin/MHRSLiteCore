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
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
    //    private readonly MyContext _myContext; >>> Protected'a çekince gerek kalmadı...
        public PatientRepository(MyContext myContext) : base(myContext)
        {

        }
    }
}
