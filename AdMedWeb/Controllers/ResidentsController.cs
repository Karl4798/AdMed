﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdMedWeb.Models;
using AdMedWeb.Repository.IRepository;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Security.Claims;

namespace AdMedWeb.Controllers
{
    [Authorize]
    public class ResidentsController : Controller
    {
        // Injected dependencies
        private readonly IResidentRepository _reRepo;
        private readonly IApplicationRepository _apRepo;
        private readonly IAccountRepository _accRepo;
        private readonly IEmailSender _emailSender;

        // GUID used for contact page
        private static Guid guid;
        private static int _applicationId;
        private static DateTime _timeStamp;
        private static Application _application;

        public ResidentsController(IResidentRepository reRepo, IApplicationRepository apRepo, IAccountRepository accRepo, IEmailSender emailSender)
        {
            _reRepo = reRepo;
            _apRepo = apRepo;
            _accRepo = accRepo;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View(new Resident() {});
        }

        [AllowAnonymous]
        public async Task<IActionResult> Upsert(int? id)
        {
            Resident obj = new Resident();
            PrimaryContactResident pcr = new PrimaryContactResident();
            if (TempData.Get<Application>("application") != null)
            {
                var app = TempData.Get<Application>("application");
                _applicationId = app.Id;
                // Resident Information
                obj.FirstName = app.FirstName;
                obj.LastName = app.LastName;
                obj.Gender = app.Gender;
                obj.Allergies = app.Allergies;
                obj.DateOfBirth = app.DateOfBirth.Date;
                obj.IdentityNumber = app.IdentityNumber;
                obj.MedicalAid = app.MedicalAid;
                obj.MedicalAidNumber = app.MedicalAidNumber;
                obj.DoctorName = app.DoctorName;
                obj.HomeTelephoneNumber = app.HomeTelephoneNumber;
                obj.WorkTelephoneNumber = app.WorkTelephoneNumber;
                obj.CellTelephoneNumber = app.CellTelephoneNumber;
                obj.Undertaker = app.Undertaker;
                obj.UndertakerTelephoneNumber = app.UndertakerTelephoneNumber;
                obj.PharmacyName = app.PharmacyName;
                obj.PharmacyTelephoneNumber = app.PharmacyTelephoneNumber;
                obj.PharmacyFaxNumber = app.PharmacyFaxNumber;
                // Primary Contact
                pcr.FirstName = app.PrimaryContact.FirstName;
                pcr.LastName = app.PrimaryContact.LastName;
                pcr.Relationship = app.PrimaryContact.Relationship;
                pcr.PhysicalAddress = app.PrimaryContact.PhysicalAddress;
                pcr.PostalAddress = app.PrimaryContact.PostalAddress;
                pcr.PostCode = app.PrimaryContact.PostCode;
                pcr.IdentityNumber = app.PrimaryContact.IdentityNumber;
                pcr.HomeTelephoneNumber = app.PrimaryContact.HomeTelephoneNumber;
                pcr.WorkTelephoneNumber = app.PrimaryContact.WorkTelephoneNumber;
                pcr.CellTelephoneNumber = app.PrimaryContact.CellTelephoneNumber;
                pcr.Email = app.PrimaryContact.Email;
                obj.PrimaryContact = pcr;

                _application = app;
            }
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
        private async Task<Resident> Update(Resident obj, int? id)
        {
            obj = await GetUpdate(obj, id);
            return obj;
        }

        [Authorize(Roles = "Admin")]
        public async Task<Resident> GetUpdate(Resident obj, int? id)
        {
            // Flow will come here for Update
            obj = await _reRepo.GetAsync(SD.ResidentAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            return obj;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Resident")]
        public async Task<ActionResult> Details()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var residents = await _reRepo.GetAllAsync(SD.ResidentAPIPath, HttpContext.Session.GetString("JWToken"));

            foreach (var item in residents)
            {
                if (item.PrimaryContact.Email.Equals(username))
                {

                    var data = await _reRepo.GetAsync(SD.ResidentAPIPath, item.Id, HttpContext.Session.GetString("JWToken"));
                    data.DateOfBirthString = data.DateOfBirth.ToString().Split(" ")[0];
                    return View(data);
                }
            }

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Upsert(Resident obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    if (await _reRepo.CreateAsync(SD.ResidentAPIPath, obj, HttpContext.Session.GetString("JWToken")))
                    {
                        // Gets a new GUID for the contact form
                        guid = Guid.NewGuid();
                        // Sends an email with all required information
                        await _emailSender.SendEmailAsync("admin@testsetup.net", "Reference Number: "
                                                                                + guid, "<h2>Email: " + obj.PrimaryContact.Email + "</h2>"
                                                                                        + "<br>" + "<h2>Your Application Was Successful!</h2>" +
                                                                                        "<p>" + obj.PrimaryContact.FirstName + " " + obj.PrimaryContact.LastName + " will be contacted shortly for further instruction.</p>" +
                                                                                        "<p>The account below has been created for the primary contact:</p>" +
                                                                                        "<p>Username: " + obj.PrimaryContact.Email + "<br>" + "Password:" + guid + "</p>" +
                                                                                        "<p>You can access your account on https://admedtech.com/ </p>");
                        // Sends an email with all required information
                        await _emailSender.SendEmailAsync(obj.PrimaryContact.Email, "Reference Number: "
                                                                                    + guid, "<h2>Email: " + obj.PrimaryContact.Email + "</h2>"
                                                                                            + "<br>" + "<h2>Your Application Was Successful!</h2>" +
                                                                                            "<p>The account below has been created for the primary contact:</p>" +
                                                                                            "<p>" + obj.PrimaryContact.FirstName + " " + obj.PrimaryContact.LastName + " will be contacted shortly for further instruction.</p>" +
                                                                                            "<p>Username: " + obj.PrimaryContact.Email + "<br>" + "Password:" + guid + "</p>" +
                                                                                            "<p>You can access your account on https://admedtech.com/ </p>");

                        if (_application != null)
                        {
                            _application.Invisible = true;
                            await _apRepo.UpdateAsync(SD.ApplicationAPIPath + _application.Id, _application, HttpContext.Session.GetString("JWToken"));
                        }

                        User user = new User()
                        {
                            FirstName = obj.PrimaryContact.FirstName,
                            LastName = obj.PrimaryContact.LastName,
                            Username = obj.PrimaryContact.Email,
                            Password = guid.ToString(),
                            Role = "Resident",
                            ConfirmPassword = guid.ToString()
                        };

                        await _accRepo.RegisterAsync(SD.AccountAPIPath + "register/", user);

                    }
                }
                else
                {
                    await PostUpdateResident(obj);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        [Authorize(Roles = "Admin")]
        public async Task PostUpdateResident(Resident obj)
        {
            await _reRepo.UpdateAsync(SD.ResidentAPIPath + obj.Id, obj, HttpContext.Session.GetString("JWToken"));
        }

        public IActionResult MedicationRedirect(Resident obj)
        {
            return RedirectToAction("Index", "Medications", new { residentId = obj.Id });
        }

        public async Task<IActionResult> GetAllResidents()
        {

            var data = await _reRepo.GetAllAsync(SD.ResidentAPIPath, HttpContext.Session.GetString("JWToken"));

            if (data != null)
            {
                foreach (var item in data)
                {
                    item.DateOfBirthString = item.DateOfBirth.ToString().Split(" ")[0];
                }
            }

            return Json(new { data });

        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _reRepo.DeleteAsync(SD.ResidentAPIPath, id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }
    }
}