using MHRSLiteEntityLayer.Models;
using MHRSLiteEntityLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteBusinessLayer.Contracts
{
    public interface IAppointmentRepository : IRepositoryBase<Appointment>
    {
        //Gideceği Randevular
        List<AppointmentVM> GetUpComingAppointments(string patientid);
        //Geçmiş Randevular
        List<AppointmentVM> GetPastAppointments(string patientid);

        //Randevu aldıktan sonra email içinde pdf halinde randevu bilgilerini göndermek için randevuyu bulmamız lazım.
        AppointmentVM GetAppointmentById(string patientid, int hcid, DateTime appointmentDate, string appointmentHour);

    }
}
