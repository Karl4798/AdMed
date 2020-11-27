using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdMedWeb.Models;
using AdMedWeb.Repository.IRepository;
using Microsoft.AspNetCore.Identity.UI.Services;
using AdMedWeb.Models.Analytics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
                    obj.TimeStamp = DateTime.Now;
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
                        await PostUpdateApplication(obj);
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

            IEnumerable<Application> data = await _apRepo.GetAllAsync(SD.ApplicationAPIPath, HttpContext.Session.GetString("JWToken"));
            List<Application> applications = new List<Application>();

            if (data != null)
            {
                foreach (var item in data)
                {
                    item.TimeStampString = item.TimeStamp.ToString().Split(" ")[0];
                    item.DateOfBirthString = item.DateOfBirth.ToString().Split(" ")[0];

                    if (item.Invisible == false)
                    {
                        applications.Add(item);
                    }

                }

                data = applications.AsEnumerable();

            }

            return Json(new { data });
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

        [Authorize(Roles = "Admin")]
        public ActionResult MonthlyApplications()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> VisualizeApplications()
        {
            // Returns the view, passing in all applications as a JSON object
            return Json(await Applications());
        }

        [Authorize(Roles = "Admin")]
        public async Task<List<MonthlyApplicants>> Applications()
        {

            // Get current date / time in order to only get months from this year
            DateTime now = DateTime.Today;

            // Creates a list of MonthlyApplicants objects
            List<MonthlyApplicants> monthlyApplicants = new List<MonthlyApplicants>();

            // Variable used to store applicants for the months
            int noOfApplications = 0;

            IEnumerable<Application> applications = await _apRepo.GetAllAsync(SD.ApplicationAPIPath, HttpContext.Session.GetString("JWToken"));

            if (applications != null)
            {
                List<int> months = new List<int>();
                List<string> monthsString = new List<string>();

                foreach (var item in applications)
                {
                    if (item.TimeStamp.Year == now.Year)
                    {
                        months.Add(item.TimeStamp.Month);
                    }
                }

                months = months.Distinct().OrderBy(x => x).ToList();

                foreach (var item in months)
                {
                    switch (item)
                    {
                        case 1:monthsString.Add("January"); break;
                        case 2:monthsString.Add("February"); break;
                        case 3:monthsString.Add("March"); break;
                        case 4:monthsString.Add("April"); break;
                        case 5:monthsString.Add("May"); break;
                        case 6:monthsString.Add("June"); break;
                        case 7:monthsString.Add("July"); break;
                        case 8:monthsString.Add("August"); break;
                        case 9:monthsString.Add("September"); break;
                        case 10:monthsString.Add("October"); break;
                        case 11:monthsString.Add("November"); break;
                        case 12:monthsString.Add("December"); break;
                    }
                }

                int index = 0;

                // Runs foreach methods which extract applicant information and increments the noOfApplications for the months in the current year
                foreach (var c in months)
                {
                    Debug.WriteLine(monthsString[index]);
                    foreach (var item in applications)
                    {
                        if (item.TimeStamp.Month == c)
                        {
                            noOfApplications++;
                        }
                    }

                    // Add new sales object (with product category and number of sales)
                    monthlyApplicants.Add(new MonthlyApplicants { Month = monthsString[index], Applicants = noOfApplications });
                    index++;

                    // Reset sales counter for next category
                    noOfApplications = 0;
                }

            }

            // Return the number of sales for the category
            return monthlyApplicants;

        }
    }
}