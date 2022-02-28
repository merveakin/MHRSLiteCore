using AutoMapper;
using MHRSLiteBusinessLayer.Contracts;
using MHRSLiteDataAccessLayer;
using MHRSLiteEntityLayer.Enums;
using MHRSLiteEntityLayer.IdentityModels;
using MHRSLiteEntityLayer.Models;
using MHRSLiteEntityLayer.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteBusinessLayer.Implementations
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        //GLOBAL ALAN
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public AppointmentRepository(MyContext myContext,
            IMapper mapper, UserManager<AppUser> userManager
            ) : base(myContext)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public AppointmentVM GetAppointmentByID(string patientid, int hcid, DateTime appointmentDate, string appointmentHour)
        {
            try
            {
                var data = GetFirstOrDefault(x =>
                           x.PatientId == patientid
                           && x.HospitalClinicId == hcid
                           && x.AppointmentDate == appointmentDate
                           && x.AppointmentHour == appointmentHour,
                           includeProperties: "HospitalClinic,Patient");

                if (data != null)
                {
                    //Hastane
                    data.HospitalClinic.Hospital
                        = _myContext.Hospitals
                        .FirstOrDefault(x => x.Id == data.HospitalClinic.HospitalId);
                    //Klinik
                    data.HospitalClinic.Clinic
                        = _myContext.Clinics
                        .FirstOrDefault(x => x.Id == data.HospitalClinic.ClinicId);
                    //İlçe
                    data.HospitalClinic.Hospital.HospitalDistrict
                        = _myContext.Districts
                        .FirstOrDefault(x => x.Id == data.HospitalClinic.Hospital.DistrictId);
                    //İl
                    data.HospitalClinic.Hospital.HospitalDistrict.City
                        = _myContext.Cities
                        .FirstOrDefault(x => x.Id == data.HospitalClinic.Hospital.HospitalDistrict.CityId);
                    //Doktor
                    data.HospitalClinic.Doctor
                        = _myContext.Doctors
                        .FirstOrDefault(x => x.TCNumber == data.HospitalClinic.DoctorId);
                    //AppUser --> tcnumber, username olarak appuserda kayıtlıdır.
                    data.HospitalClinic.Doctor.AppUser = _userManager
                        .FindByNameAsync(data.HospitalClinic.DoctorId).Result;

                    var returnData = _mapper.Map<Appointment, AppointmentVM>(data);
                    return returnData;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<AppointmentVM> GetPastAppointments(string patientid)
        {
            try
            {
                var data = GetAll(x =>
                x.PatientId == patientid
                && x.AppointmentStatus != AppointmentStatus.Active,
                includeProperties: "HospitalClinic,Patient").ToList();

                foreach (var item in data)
                {
                    //Hastane
                    item.HospitalClinic.Hospital
                        = _myContext.Hospitals
                        .FirstOrDefault(x => x.Id == item.HospitalClinic.HospitalId);
                    //Klinik
                    item.HospitalClinic.Clinic
                        = _myContext.Clinics
                        .FirstOrDefault(x => x.Id == item.HospitalClinic.ClinicId);
                    //İlçe
                    item.HospitalClinic.Hospital.HospitalDistrict
                        = _myContext.Districts
                        .FirstOrDefault(x => x.Id == item.HospitalClinic.Hospital.DistrictId);
                    //İl
                    item.HospitalClinic.Hospital.HospitalDistrict.City
                        = _myContext.Cities
                        .FirstOrDefault(x => x.Id == item.HospitalClinic.Hospital.HospitalDistrict.CityId);
                    //Doktor
                    item.HospitalClinic.Doctor
                        = _myContext.Doctors
                        .FirstOrDefault(x => x.TCNumber == item.HospitalClinic.DoctorId);
                    //AppUser --> tcnumber, username olarak appuserda kayıtlıdır.
                    item.HospitalClinic.Doctor.AppUser = _userManager
                        .FindByNameAsync(item.HospitalClinic.DoctorId).Result;
                }
                var returnData =
                    _mapper.Map<List<Appointment>, List<AppointmentVM>>(data);
                return returnData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AppointmentVM> GetUpComingAppointments(string patientid)
        {
            try
            {
                var data = GetAll(x =>
                x.PatientId == patientid
                && x.AppointmentStatus == AppointmentStatus.Active,
                includeProperties: "HospitalClinic,Patient").ToList();

                foreach (var item in data)
                {
                    //Hastane
                    item.HospitalClinic.Hospital
                        = _myContext.Hospitals
                        .FirstOrDefault(x => x.Id == item.HospitalClinic.HospitalId);
                    //Klinik
                    item.HospitalClinic.Clinic
                        = _myContext.Clinics
                        .FirstOrDefault(x => x.Id == item.HospitalClinic.ClinicId);
                    //İlçe
                    item.HospitalClinic.Hospital.HospitalDistrict
                        = _myContext.Districts
                        .FirstOrDefault(x => x.Id == item.HospitalClinic.Hospital.DistrictId);
                    //İl
                    item.HospitalClinic.Hospital.HospitalDistrict.City
                        = _myContext.Cities
                        .FirstOrDefault(x => x.Id == item.HospitalClinic.Hospital.HospitalDistrict.CityId);
                    //Doktor
                    item.HospitalClinic.Doctor
                        = _myContext.Doctors
                        .FirstOrDefault(x => x.TCNumber == item.HospitalClinic.DoctorId);
                    //AppUser --> tcnumber, username olarak appuserda kayıtlıdır.
                    item.HospitalClinic.Doctor.AppUser = _userManager
                        .FindByNameAsync(item.HospitalClinic.DoctorId).Result;
                }
                var returnData =
                    _mapper.Map<List<Appointment>, List<AppointmentVM>>(data);
                return returnData;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
