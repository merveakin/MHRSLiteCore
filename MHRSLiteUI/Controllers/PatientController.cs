using MHRSLiteBusinessLayer.Contracts;
using MHRSLiteBusinessLayer.EmailService;
using MHRSLiteEntityLayer.IdentityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
        public IActionResult Index()
        {
            try
            {

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [Authorize]
        public IActionResult Appointment()
        {
            try
            {
                ViewBag.Cities = _unitOfWork.CityRepository.GetAll(orderBy:x=>x.OrderBy(a=>a.CityName));


                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

    }
}
