using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AdMedWeb.Models;
using AdMedWeb.Models.ViewModels;
using AdMedWeb.Repository.IRepository;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace AdMedWeb.Controllers
{
    public class HomeController : Controller
    {

        private readonly IAccountRepository _accRepo;
        private readonly IEmailSender _emailSender;

        // GUID used for contact page
        private static Guid guid;

        public HomeController(ILogger<HomeController> logger, IApplicationRepository npRepo,
            IEmergencyContactRepository emRepo, IAccountRepository accRepo, IEmailSender emailSender)
        {

            _accRepo = accRepo;
            _emailSender = emailSender;

        }

        public async Task<IActionResult> Index()
        {

            return View();
        }

        [HttpGet]
        public ViewResult About()
        {
            return View();
        }

        // Method used to post the Contact page
        [HttpPost]
        public async Task<IActionResult> About(ContactForm model)
        {

            // If the model state is valid (all fields have passed validation), then send a message to admin@testsetup.net
            if (ModelState.IsValid)
            {

                // Gets a new GUID for the contact form
                guid = Guid.NewGuid();

                // Sends the email with all required information
                await _emailSender.SendEmailAsync("admin@testsetup.net", "Reference Number: "
                                                                         + guid, "<h2>Email: " + model.Email + "</h2>"
                                                                                 + "<br>" + "<h2>Message</h2>" +
                                                                                 "<p>" + model.Message.Replace("\n", "<br>") + "</p>");

                await _emailSender.SendEmailAsync(model.Email, "Reference Number: "
                                                                         + guid, "<h2>Email: " + model.Email + "</h2>"
                                                                                 + "<br>" + "<h2>Message</h2>" +
                                                                                 "<p>" + model.Message.Replace("\n", "<br>") + "</p>");


                // Return confirmation page
                return RedirectToAction("MessageSubmitted");
            }

            // Returns the contact page
            return About();
        }

        [HttpGet]
        public ViewResult MessageSubmitted()
        {

            // Sets the ViewBag id equal to the sent GUID
            ViewBag.id = guid;

            // Returns the view
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Login()
        {
            User obj = new User();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User obj)
        {
            User objUser = await _accRepo.LoginAsync(SD.AccountAPIPath + "authenticate/", obj);
            if (objUser.Token == null)
            {
                TempData["Error"] = "Incorrect Username / Password Combination.";
                return View();
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, objUser.Username));
            identity.AddClaim(new Claim(ClaimTypes.Role, objUser.Role));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            HttpContext.Session.SetString("JWToken", objUser.Token);
            TempData["alert"] = "Welcome " + objUser.Username;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User obj)
        {
            bool result = await _accRepo.RegisterAsync(SD.AccountAPIPath + "register/", obj);
            if (result == false)
            {
                return View();
            }
            TempData["alert"] = "Registration Successful";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("JWToken", "");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
