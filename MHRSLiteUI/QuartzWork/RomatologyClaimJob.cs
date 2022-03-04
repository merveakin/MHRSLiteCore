using MHRSLiteBusinessLayer.Contracts;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MHRSLiteEntityLayer.Enums;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MHRSLiteEntityLayer.IdentityModels;

namespace MHRSLiteUI.QuartzWork
{
    public class RomatologyClaimJob : IJob
    {
        private readonly ILogger<RomatologyClaimJob> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public RomatologyClaimJob(
            ILogger<RomatologyClaimJob> logger
            , IUnitOfWork unitOfWork
            , UserManager<AppUser> userManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var date = DateTime.Now.AddMonths(-1);
                //son bir aydaki dahiliyedeki iptal olan hariç tüm randevuları getir.
                var appointment = _unitOfWork
                    .AppointmentRepository.GetAppointmentsIM(date).OrderByDescending(x => x.AppointmentDate).ToList();

                foreach (var item in appointment)
                {
                    //Usera ait Dahiliye-Romatoloji claimi yoksa eklenmeli...
                    //04:03:2022 DEVAM
                    //Claim Varsa tarihi aynı mı ? tarihi aynıdeğilse tarihi replace yap
                    //Claim yoksa yeni claim ekle

                    //Romatology Claim
                    var claimValue = $"{item.HospitalClinicId}_" +
                        $"{item.AppointmentDate.ToString("dd/MM/yyyy")}";
                    Claim romatologyClaim = new Claim("DahiliyeRomatoloji",
                        claimValue, ClaimValueTypes.String, "Internal");

                    //userın claim listesini alalım ve control edelim
                    var claimList = await _userManager.GetClaimsAsync(item.Patient.AppUser);
                    var claim = claimList.FirstOrDefault(x => x.Type == "DahiliyeRomatoloji");

                    if (claim == null)
                    {
                        //Claim yoksa Claim ekleyelim
                        await _userManager.AddClaimAsync(item.Patient.AppUser, romatologyClaim);
                    }

                    else
                    {
                        //Eğer Claim varsa...
                        //Claimdeki değerlere bakılır...

                        // int claimHCID = Convert.ToInt32(
                        //claim.Value.Substring(0, claim.Value.IndexOf('_')));
                        // DateTime claimDate = Convert.ToDateTime(
                        //      claim.Value.Substring(claim.Value.IndexOf('_') + 1).ToString());
                        // //yöntem 2
                        string[] array = claim.Value.Split('_');
                        int claimHCID = Convert.ToInt32(array[0]);
                        DateTime claimDate = Convert.ToDateTime(array[1].ToString());
                        if (claimDate < item.AppointmentDate)
                        {
                            await _userManager.ReplaceClaimAsync(item.Patient.AppUser, claim, romatologyClaim);
                        }
                    }
                }

                _logger.LogInformation("RomatologyClaims updated");


            }
            catch (Exception ex)
            {

                //loglanacak

            }
        }
    }
}
