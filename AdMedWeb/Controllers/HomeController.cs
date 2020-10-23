using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdMedWeb.Models;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace AdMedWeb.Controllers
{
    public class HomeController : Controller
    {

        private readonly IEmailSender _emailSender;

        // GUID used for contact page
        private static Guid guid;

        public HomeController(IEmailSender emailSender)
        {

            _emailSender = emailSender;

        }

        public IActionResult Index()
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
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
