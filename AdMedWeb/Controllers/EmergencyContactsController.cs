using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AdMedWeb.Models;
using AdMedWeb.Models.ViewModels;
using AdMedWeb.Repository.IRepository;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json;

namespace AdMedWeb.Controllers
{
    [Authorize]
    public class EmergencyContactsController : Controller
    {

        private readonly IEmergencyContactRepository _emRepo;
        private readonly IApplicationRepository _apRepo;
        private readonly IEmailSender _emailSender;
        private static Application application { get; set; }

        // GUID used for contact page
        private static Guid guid;

        public EmergencyContactsController(IApplicationRepository apRepo,
            IEmergencyContactRepository emRepo, IEmailSender emailSender)
        {
            _apRepo = apRepo;
            _emRepo = emRepo;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {

            return View(new Application() {});
        }

        [AllowAnonymous]
        public async Task<IActionResult> Upsert(int? id)
        {

            //await _apRepo.CreateAsync(SD.ApplicationAPIPath, data, HttpContext.Session.GetString("JWToken"));

            Application appTemp = TempData.Get<Application>("Application");
            application = appTemp;

            //await _apRepo.CreateAsync(SD.ApplicationAPIPath, appTemp, HttpContext.Session.GetString("JWToken"));

            EmergencyContact objVM = new EmergencyContact();
            if (id == null)
            {

                // This will be true for Insert / Create
                return View(objVM);
            }

            // Flow will come here for Update
            objVM = await _emRepo.GetAsync(SD.EmergencyContactAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));

            if (objVM == null)
            {
                return NotFound();
            }

            return View(objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Upsert(EmergencyContact obj)
        {

            if (ModelState.IsValid)
            {
                obj.ApplicationId = 1;
                if (obj.Id == 0)
                {
                    await _apRepo.CreateAsync(SD.ApplicationAPIPath, application, HttpContext.Session.GetString("JWToken"));

                    IEnumerable<Application> a = await _apRepo.GetAllAsync(SD.ApplicationAPIPath, HttpContext.Session.GetString("JWToken"));
                    foreach (var VARIABLE in a)
                    {
                        if (VARIABLE.IdentityNumber == application.IdentityNumber)
                        {
                            obj.ApplicationId = VARIABLE.Id;
                        }
                    }

                    await _emRepo.CreateAsync(SD.EmergencyContactAPIPath, obj, HttpContext.Session.GetString("JWToken"));

                    // Gets a new GUID for the contact form
                    guid = Guid.NewGuid();

                    // Sends the email with all required information
                    await _emailSender.SendEmailAsync("admin@testsetup.net", "Reference Number: "
                                                                             + guid, "<h2>Email: " + obj.Email + "</h2>"
                                                                                     + "<br>" + "<h2>Message</h2>" +
                                                                                     "<p>" + "New Application + " + "<br>" + obj.ApplicationId + "</p>");

                    await _emailSender.SendEmailAsync(obj.Email, "Reference Number: "
                                                                   + guid, "<h2>Email: " + obj.Email + "</h2>"
                                                                           + "<br>" + "<h2>Message</h2>" +
                                                                           "<p>" + "New Application + " + "<br>" + obj.ApplicationId + "</p>");
                }
                else
                {
                    await _emRepo.UpdateAsync(SD.EmergencyContactAPIPath + obj.Id, obj, HttpContext.Session.GetString("JWToken"));
                }

                return RedirectToAction(nameof(Index));

            }

            return View(obj);

        }

        public async Task<IActionResult> GetAllTrails()
        {
            return Json(new {data = await _emRepo.GetAllAsync(SD.EmergencyContactAPIPath, HttpContext.Session.GetString("JWToken")) });
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _emRepo.DeleteAsync(SD.EmergencyContactAPIPath, id, HttpContext.Session.GetString("JWToken"));

            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }

            return Json(new { success = false, message = "Delete Not Successful" });
        }

    }

}
