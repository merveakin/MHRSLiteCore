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
    public class DistrictRepository : Repository<District>, IDistrictRepository
    {
        private readonly MyContext _myContext;
        public DistrictRepository(MyContext myContext) : base(myContext)
        {

        }
    }
}
