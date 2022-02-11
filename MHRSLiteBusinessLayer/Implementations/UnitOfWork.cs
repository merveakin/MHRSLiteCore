using MHRSLiteBusinessLayer.Contracts;
using MHRSLiteDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteBusinessLayer.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyContext _myContext;
        public UnitOfWork(MyContext myContext)
        {
            _myContext = myContext;
            //UnitOfWork tüm repositoryleri oluşturacak.
            CityRepository = new CityRepository(_myContext);
            DistrictRepository = new DistrictRepository(_myContext);
            DoctorRepository = new DoctorRepository(_myContext);
            PatientRepository = new PatientRepository(_myContext);
        }

        public ICityRepository CityRepository { get; private set; }

        public IDistrictRepository DistrictRepository { get; private set; }

        public IDoctorRepository DoctorRepository { get; private set; }

        public IPatientRepository PatientRepository { get; private set; }

        public void Dispose()
        {
            _myContext.Dispose();
        }
    }
}
