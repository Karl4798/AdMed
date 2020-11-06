using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdMedWeb.Models;
using AdMedWeb.Repository.IRepository;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace AdMedWeb.Controllers
{
    [Authorize]
    public class ApplicationsController : Controller
    {
        private readonly IApplicationRepository _apRepo;
        private readonly IEmailSender _emailSender;

        // GUID used for contact page
        private static Guid guid;

        public ApplicationsController(IApplicationRepository apRepo, IEmailSender emailSender)
        {
            _apRepo = apRepo;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View(new Application() {});
        }

        [AllowAnonymous]
        public async Task<IActionResult> Upsert(int? id)
        {
            Application obj = new Application();
            if (id == null)
            {
                // This will be true for Insert / Create
                return View(obj);
            }
            obj = await Update(obj, id);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [Authorize("Admin")]
        private async Task<Application> Update(Application obj, int? id)
        {
            obj = await GetUpdate(obj, id);
            return obj;
        }

        [Authorize(Roles = "Admin")]
        public async Task<Application> GetUpdate(Application obj, int? id)
        {
            // Flow will come here for Update
            obj = await _apRepo.GetAsync(SD.ApplicationAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            return obj;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Upsert(Application obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    await _apRepo.CreateAsync(SD.ApplicationAPIPath, obj, HttpContext.Session.GetString("JWToken"));
                    // Gets a new GUID for the contact form
                    guid = Guid.NewGuid();
                    // Sends the email with all required information
                    await _emailSender.SendEmailAsync("admin@testsetup.net", "Reference Number: "
                                                                                + guid, "<h2>Email: " + obj.PrimaryContact.Email + "</h2>"
                                                                                        + "<br>" + "<h2>Your Application is Currently Pending.</h2>" +
                                                                                        "<p>" + obj.PrimaryContact.FirstName + " " + obj.PrimaryContact.LastName + " will be contacted shortly.</p>");
                    await _emailSender.SendEmailAsync(obj.PrimaryContact.Email, "Reference Number: "
                                                                                + guid, "<h2>Email: " + obj.PrimaryContact.Email + "</h2>"
                                                                                        + "<br>" + "<h2>Your Application is Currently Pending.</h2>" +
                                                                                        "<p>" + obj.PrimaryContact.FirstName + " " + obj.PrimaryContact.LastName + " will be contacted shortly.</p>");
                    if (!User.Identity.IsAuthenticated)
                    {
                        return RedirectToAction("Confirmation", "Applications", new
                        {
                            identifier = guid,
                            email = obj.PrimaryContact.Email,
                            firstName = obj.PrimaryContact.FirstName,
                            lastName = obj.PrimaryContact.LastName
                        });
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    if (obj.Approval == Enums.Approvals.Approve)
                    {
                        TempData.Put("application", obj);
                        return RedirectToAction("Upsert", "Residents");
                    }
                    if (obj.Approval == Enums.Approvals.Decline)
                    {
                        await _apRepo.DeleteAsync(SD.ApplicationAPIPath, obj.Id, HttpContext.Session.GetString("JWToken"));
                        return RedirectToAction(nameof(Index));
                    }
                    await PostUpdateApplication(obj);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(obj);
        }

        [Authorize(Roles = "Admin")]
        public async Task PostUpdateApplication(Application obj)
        {
            await _apRepo.UpdateAsync(SD.ApplicationAPIPath + obj.Id, obj, HttpContext.Session.GetString("JWToken"));
        }

        [AllowAnonymous]
        public IActionResult Confirmation(Guid identifier, string email, string firstName, string lastName)
        {
            ViewBag.guid = identifier;
            ViewBag.email = email;
            ViewBag.firstName = firstName;
            ViewBag.lastName = lastName;
            return View();
        }

        public async Task<IActionResult> GetAllApplications()
        {
            return Json(new {data = await _apRepo.GetAllAsync(SD.ApplicationAPIPath, HttpContext.Session.GetString("JWToken")) });
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _apRepo.DeleteAsync(SD.ApplicationAPIPath, id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }
    }
}