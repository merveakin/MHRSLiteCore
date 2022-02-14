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
   public class AppointmentHourRepository : Repository<AppointmentHour>, IAppointmentHourRepository
    {
        private readonly MyContext _myContext;
        
        public AppointmentHourRepository(MyContext myContext) : base(myContext)
        {
            _myContext = myContext;
        }
    }
}
