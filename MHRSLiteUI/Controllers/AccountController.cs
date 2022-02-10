using MHRSLiteBusinessLayer.EmailService;
using MHRSLiteEntityLayer;
using MHRSLiteEntityLayer.Enums;
using MHRSLiteEntityLayer.IdentityModels;
using MHRSLiteUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MHRSLiteUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailSender _emailSender;

        //Dependency Injection
        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            CheckRoles();
        }

        private void CheckRoles()
        {
            var allRoles = Enum.GetNames(typeof(RoleNames));
            foreach (var item in allRoles)
            {
                if (!_roleManager.RoleExistsAsync(item).Result)
                {
                    var result = _roleManager.CreateAsync(new AppRole()
                    {
                        Name = item,
                        Description = item
                    }).Result;
                }
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }


                var checkUserForUserName = await _userManager.FindByNameAsync(model.UserName);
                if (checkUserForUserName != null)
                {
                    ModelState.AddModelError(nameof(model.UserName), "Bu kullanıcı adı zaten sistemde kayıtlıdır.");
                    return View(model);
                }


                var checkUserForEmail = await _userManager.FindByEmailAsync(model.Email);
                if (checkUserForEmail != null)
                {
                    ModelState.AddModelError(nameof(model.Email), "Bu email zaten sistemde kayıtlıdır.");
                    return View(model);
                }

                //Yeni kullanıcı oluşturalım
                AppUser newUser = new AppUser()
                {
                    Email = model.Email,
                    Name = model.Name,
                    Surname = model.Surname,
                    UserName = model.UserName,
                    Gender = model.Gender
                    //TODO : Birthdate?
                    //TODO : Phone Number?
                };
                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(newUser, RoleNames.Patient.ToString());

                    //Email gönderilmelidir.
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callBackUrl = Url.Action("ConfirmEmail", "Account", new { userId = newUser.Id, code = code }, protocol: Request.Scheme);

                    var emailMessage = new EmailMessage()
                    {
                        Contacts = new string[] { newUser.Email },
                        Subject = "MHRSLITE - Email Aktivasyonu",
                        Body = $"Merhaba {newUser.Name} {newUser.Surname}. <br/> Hesabınızı aktifleştirmek için <a href='{HtmlEncoder.Default.Encode(callBackUrl)}'> Buraya </a> tıklayınız."
                    };

                    await _emailSender.SendAsync(emailMessage);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Beklenmedik bir hata oluştu!");
                    return View(model);
                }
            }
            catch (Exception ex)
            {

                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            try
            {
                if (userId == null || code == null)
                {
                    return NotFound("Sayfa Bulunamadı!");
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("Kullanıcı Bulunamadı!");
                }
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                //EmailConfirmed=1 ya da True
                var result = await _userManager.ConfirmEmailAsync(user, code);

                if (result.Succeeded)
                {
                    if (_userManager.IsInRoleAsync(user, RoleNames.Passive.ToString()).Result)
                    {
                        await _userManager.RemoveFromRoleAsync(user, RoleNames.Passive.ToString());
                        await _userManager.AddToRoleAsync(user, RoleNames.Patient.ToString());
                    }

                    TempData["EmailConfirmedMessage"] = "Hesabınız aktifleşmiştir...";
                    return RedirectToAction("Login", "Account");
                }

                //Login sayfasında bu tempdata view ekranında kontrol edilecektir.>>Değiştirdik ve ViewBag'le yazdık.
                ViewBag.EmailConfirmedMessage = "Hesap aktifleştirme başarısızdır!";
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                ViewBag.EmailConfirmedMessage = "Beklenmedik bir hata oldu! Tekrar deneyiniz.";
                return View();
            }
        }

    }
}
