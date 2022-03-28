using MHRSLiteBusinessLayer.Contracts;
using MHRSLiteEntityLayer.PagingListModels;
using MHRSLiteEntityLayer.ViewModels;
using MHRSLiteUI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MHRSLiteUI.Components
{
    public class PatientAppointmentsViewComponent : ViewComponent
    {
        //GLOBAL ALAN
        private readonly IUnitOfWork _unitOfWork;

        //Dependency Injection
        public PatientAppointmentsViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IViewComponentResult Invoke(int pageNumberPast = 1, int pageNumberFuture = 1)
        {
            PastAndFutureAppointmentsViewModel data =
                new PastAndFutureAppointmentsViewModel();

            //23.02.2022
            DateTime today = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            var patientId = HttpContext.User.Identity.Name;

            //Aktif randevular
            var upcomingAppointments = _unitOfWork.AppointmentRepository
                .GetUpComingAppointments(patientId);

            data.UpcomingAppointments = PaginatedList<AppointmentVM>
                .CreateAsync(upcomingAppointments, pageNumberFuture, 4);

            //Geçmiş ve İptal randevular
            var pastAndCancelledAppointments = _unitOfWork.AppointmentRepository
                .GetPastAppointments(patientId);

            data.PastAppointments = PaginatedList<AppointmentVM>
                .CreateAsync(pastAndCancelledAppointments, pageNumberPast, 4);
            //

            //
            return View(data);
        }
    }
}
