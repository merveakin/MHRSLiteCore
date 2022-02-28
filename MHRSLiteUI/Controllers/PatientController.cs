﻿using ClosedXML.Excel;
using MHRSLiteBusinessLayer.Contracts;
using MHRSLiteBusinessLayer.EmailService;
using MHRSLiteEntityLayer;
using MHRSLiteEntityLayer.Enums;
using MHRSLiteEntityLayer.IdentityModels;
using MHRSLiteEntityLayer.Models;
using MHRSLiteUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MHRSLiteUI.Controllers
{
    public class PatientController : Controller
    {
        //GLOBAL ALAN
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        //Dependency Injection
        public PatientController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            IEmailSender emailSender,
            IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [Authorize]
        public IActionResult Index(int pageNumberPast = 1, int pageNumberFuture = 1)
        {
            try
            {
                ViewBag.PageNumberPast = pageNumberPast;
                ViewBag.PageNumberFuture = pageNumberFuture;

                return View();
            }
            catch (Exception)
            {
                return View();
            }
        }

        [Authorize]
        public IActionResult Appointment()
        {
            try
            {
                ViewBag.Cities = _unitOfWork.CityRepository.GetAll(orderBy: x => x.OrderBy(a => a.CityName));

                ViewBag.Clinics = _unitOfWork.ClinicRepository.GetAll(orderBy: x => x.OrderBy(a => a.ClinicName));

                return View();
            }
            catch (Exception)
            {
                return View();
            }
        }

        [Authorize]
        public IActionResult FindAppointment(int cityid, int? distid,
              int cid, int? hid, int? dr)
        {
            try
            {
                TempData["ClinicId"] = cid;
                TempData["HospitalId"] = hid.Value;

                //Dışarıdan gelen hid ve clinicid'nin olduğu HospitalClinic kayıtlarını al
                var data = _unitOfWork.HospitalClinicRepository
                    .GetAll(x => x.ClinicId == cid
                    && x.HospitalId == hid.Value)
                    .Select(a => a.AppointmentHours)
                    .ToList();

                var list = new List<AvailableDoctorAppointmentViewModel>();

                foreach (var item in data)
                {
                    foreach (var subitem in item)
                    {
                        var hospitalClinicData =
                            _unitOfWork.HospitalClinicRepository
                            .GetFirstOrDefault(x => x.Id == subitem.HospitalClinicId);

                        var hours = subitem.Hours.Split(',');
                        var appointment = _unitOfWork
                            .AppointmentRepository
                            .GetAll(
                            x => x.HospitalClinicId == subitem.HospitalClinicId
                            &&
                            (x.AppointmentDate > DateTime.Now.AddDays(-1)
                            &&
                            x.AppointmentDate < DateTime.Now.AddDays(2)
                            )
                            ).ToList();
                        foreach (var houritem in hours)
                        {
                            if (appointment.Count(
                                x =>
                                x.AppointmentDate == (
                                Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString())) &&
                                x.AppointmentHour == houritem
                                ) == 0)
                            {
                                list.Add(new AvailableDoctorAppointmentViewModel()
                                {
                                    HospitalClinicId = subitem.HospitalClinicId,
                                    ClinicId = hospitalClinicData.ClinicId,
                                    HospitalId = hospitalClinicData.HospitalId,
                                    DoctorTCNumber = hospitalClinicData.DoctorId,
                                    Doctor = _unitOfWork.DoctorRepository
                                    .GetFirstOrDefault(x => x.TCNumber ==
                                    hospitalClinicData.DoctorId, includeProperties: "AppUser"),
                                    Hospital = _unitOfWork.HospitalRepository
                                    .GetFirstOrDefault(x => x.Id ==
                                    hospitalClinicData.HospitalId),
                                    Clinic = _unitOfWork.ClinicRepository
                                    .GetFirstOrDefault(x => x.Id == hospitalClinicData.ClinicId),
                                    HospitalClinic = hospitalClinicData
                                });
                                break;
                            }
                        }
                    }
                }
                list = list.Distinct().OrderBy(x => x.Doctor.AppUser.Name).ToList();
                return View(list);


            }
            catch (Exception)
            {

                throw;
            }

        }

        [Authorize]
        public IActionResult FindAppointmentHours(int hcid)
        {
            try
            {

                var list = new List<AvailableDoctorAppointmentHoursViewModel>();

                var data = _unitOfWork.
                    AppointmentHourRepository.
                    GetFirstOrDefault(x => x.HospitalClinicId == hcid);

                var hospitalClinicData =
                         _unitOfWork.HospitalClinicRepository
                         .GetFirstOrDefault(x => x.Id == hcid);
                Doctor dr = _unitOfWork.DoctorRepository
                    .GetFirstOrDefault(x => x.TCNumber == hospitalClinicData.DoctorId
                    , includeProperties: "AppUser");
                ViewBag.Doctor = "Dr." + dr.AppUser.Name + " " + dr.AppUser.Surname;


                var hours = data.Hours.Split(',');

                var appointment = _unitOfWork
                    .AppointmentRepository
                    .GetAll(
                    x => x.HospitalClinicId == hcid
                    &&
                    (x.AppointmentDate > DateTime.Now.AddDays(-1)
                    &&
                    x.AppointmentDate < DateTime.Now.AddDays(2)
                    )
                    ).ToList();

                foreach (var houritem in hours)
                {
                    string myHourBase = houritem.Substring(0, 2) + ":00";
                    var appointmentHourData =
                      new AvailableDoctorAppointmentHoursViewModel()
                      {
                          AppointmentDate = DateTime.Now.AddDays(1),
                          Doctor = dr,
                          HourBase = myHourBase,
                          HospitalClinicId = hcid
                      };
                    if (list.Count(x => x.HourBase == myHourBase) == 0)
                    {
                        list.Add(appointmentHourData);
                    }
                    if (appointment.Count(
                        x =>
                        x.AppointmentDate == (
                        Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString())) &&
                        x.AppointmentHour == houritem
                        ) == 0)
                    {
                        if (list.Count(x => x.HourBase == myHourBase) > 0)
                        {
                            list.Find(x => x.HourBase == myHourBase
                                ).Hours.Add(houritem);
                        }
                    }
                }
                return View(list);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        public IActionResult FindAppointment_OncekiVersiyon(int cityid, int? distid, int cid, int? hid, int? dr)
        {
            try
            {
                //Dışarıdan gelen ClinicId ' nin olduğu HospitalClinic kayıtlarını al.
                var data = _unitOfWork.HospitalClinicRepository
                    .GetAll(x => x.ClinicId == cid && x.HospitalId == hid.Value)
                    .Select(a => a.AppointmentHours)
                    .ToList();

                var list = new List<PatientAppointmentViewModel>();
                foreach (var item in data)
                {
                    foreach (var subitem in item)
                    {
                        var hospitalClinicData =
                            _unitOfWork.HospitalClinicRepository
                            .GetFirstOrDefault(x => x.Id == subitem.HospitalClinicId);

                        var hours = subitem.Hours.Split(',');
                        var appointment = _unitOfWork
                            .AppointmentRepository
                            .GetAll(
                            x =>
                            x.HospitalClinicId == subitem.HospitalClinicId
                            &&
                            (x.AppointmentDate > DateTime.Now.AddDays(-1)
                            &&
                            x.AppointmentDate < DateTime.Now.AddDays(2)
                            )
                            ).ToList();


                        foreach (var houritem in hours)
                        {
                            if (appointment.Count(
                                x =>
                                x.AppointmentDate == (Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString())) &&
                                x.AppointmentHour == houritem
                                ) == 0)
                            {
                                list.Add(new PatientAppointmentViewModel()
                                {
                                    AppointmentDate =
                                    Convert.ToDateTime(DateTime.Now.AddDays(1)),
                                    HospitalClinicId = subitem.HospitalClinicId,
                                    DoctorId = hospitalClinicData.DoctorId,
                                    AvailableHour = houritem,
                                    Doctor = _unitOfWork.
                                    DoctorRepository
                                    .GetFirstOrDefault(x =>
                                    x.TCNumber == hospitalClinicData.DoctorId,
                                    includeProperties: "AppUser")
                                });


                            }
                        }
                    }
                }

                list = list.Distinct().OrderBy(x => x.AppointmentDate).ToList();
                return View(list);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        public JsonResult SaveAppointment(int hcid, string date, string hour)
        {
            var message = string.Empty;
            try
            {
                //Aynı tarih ve saate randevusu var mı?
                DateTime appointmentDate = Convert.ToDateTime(date);
                if (_unitOfWork.AppointmentRepository.GetFirstOrDefault(x => x.AppointmentDate == appointmentDate && x.AppointmentHour == hour) != null)
                {
                    //Aynı tarihe ve saate başka randevusu var
                    message = $"{date} - {hour} tarihinde bir kliniğe zaten randevu almışsınız. Aynı tarih ve saate başka randevu alamazsınız!";
                    return Json(new { isSuccess = false, message });
                }

                //randevu kayıt edilecek
                Appointment patientAppointment = new Appointment()
                {
                    CreatedDate = DateTime.Now,
                    PatientId = HttpContext.User.Identity.Name,
                    HospitalClinicId = hcid,
                    AppointmentDate = appointmentDate,
                    AppointmentHour = hour,
                    AppointmentStatus = AppointmentStatus.Active
                };

                bool result = _unitOfWork.AppointmentRepository.Add(patientAppointment);


                message =
                    result ? "Randevunuz başarıyla kaydolmuştur."
                    : "HATA : Beklenmedik bir hata oluştu!";

                if (result)
                {
                    //Randevu bilgilerini pdf olarak emaille gönderilmesi isteniyor.

                    var data = _unitOfWork.AppointmentRepository
                        .GetAppointmentByID(
                        HttpContext.User.Identity.Name,
                        patientAppointment.HospitalClinicId,
                        patientAppointment.AppointmentDate,
                        patientAppointment.AppointmentHour
                        );
                    var user = _userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
                    var emailMessage = new EmailMessage()
                    {
                        Contacts = new string[] { user.Email },
                        Subject = "MHRSLITE - Randevu Bilgileri",
                        Body = $"{user.Name} {user.Surname}, <br/>randevu bilgileriniz pdf olarak ektedir."
                    };
                    _emailSender.SendAppointmentPdf(emailMessage, data);
                }

                return result ? Json(new { isSuccess = true, message })
                              : Json(new { isSuccess = false, message });

            }
            catch (Exception ex)
            {

                message = "HATA : " + ex.Message;

                return Json(new
                {
                    isSuccess = false,
                    message
                });

            }
        }

        [Authorize]
        public JsonResult CancelAppointment(int id)
        {
            var message = string.Empty;
            try
            {
                var appointment = _unitOfWork
                    .AppointmentRepository
                    .GetFirstOrDefault(x => x.Id == id);

                if (appointment != null)
                {
                    appointment.AppointmentStatus = AppointmentStatus.Cancelled;
                    var result = _unitOfWork.AppointmentRepository
                        .Update(appointment);
                    message = result ?
                        "Randevunuz iptal edildi!"
                        :
                        "HATA : Beklenmedik bir sorun oluştu!";

                    return result ?
                        Json(new { isSuccess = true, message })
                        :
                        Json(new { isSuccess = false, message });
                }
                else
                {
                    message = "HATA : Randevu bulunamadığı için iptal edilemedi! Tekrar deneyiniz!";
                    return Json(new { isSuccess = false, message });
                }
            }
            catch (Exception ex)
            {

                message = "HATA : " + ex.Message;
                return Json(new { isSuccess = false, message });
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult UpcomingAppointmentsExcelExport()
        {
            try
            {
                DataTable dt = new DataTable();
                var patientId = HttpContext.User.Identity.Name;
                var data = _unitOfWork.AppointmentRepository
                    .GetUpComingAppointments(patientId);
                dt.Columns.Add("İL");
                dt.Columns.Add("İLÇE");
                dt.Columns.Add("HASTANE");
                dt.Columns.Add("KLİNİK");
                dt.Columns.Add("DOKTOR");
                dt.Columns.Add("RANDEVU TARİHİ");
                dt.Columns.Add("RANDEVU SAATİ");

                foreach (var item in data)
                {
                    var Doktor =
                        item.HospitalClinic.Doctor.AppUser.Name + " "
                        + item.HospitalClinic.Doctor.AppUser.Surname;
                    dt.Rows.Add(
                                                                                    item.HospitalClinic.Hospital.HospitalDistrict.City.CityName,
                        item.HospitalClinic.Hospital.HospitalDistrict.DistrictName,
                        item.HospitalClinic.Hospital.HospitalName,
                        item.HospitalClinic.Clinic.ClinicName,
                        Doktor,
                        item.AppointmentDate,
                        item.AppointmentHour);
                }
                //EXCEL oluşturacak.
                using (XLWorkbook wb= new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        //return File ile dosya tarayıcınıza kaydolur.
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
