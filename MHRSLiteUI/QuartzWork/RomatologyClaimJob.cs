using MHRSLiteBusinessLayer.Contracts;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MHRSLiteEntityLayer.Enums;

namespace MHRSLiteUI.QuartzWork
{
    public class RomatologyClaimJob : IJob
    {
        private readonly ILogger<RomatologyClaimJob> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public RomatologyClaimJob(ILogger<RomatologyClaimJob> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var date = DateTime.Now.AddMonths(-1);
                //son bir aydaki dahiliyedeki iptal olan hariç tüm randevuları getir.
                var appointment = _unitOfWork
                    .AppointmentRepository.GetAppointmentsIM(date).OrderByDescending(x => x.AppointmentDate).ToList();

                foreach (var item in appointment)
                {
                    //usera ait Dahiliye-Romatoloji claimi yoksa eklenmeli...
                    //03:03:2022 DEVAM
                    //claim Varsa tarihi aynı mı ? tarihi aynıdeğilse sil ve yenidem ekle
                    //yoksa yeni claim ekle
                }
               
            }
            catch (Exception)
            {
                //loglanacak

            }
        }
    }
}
