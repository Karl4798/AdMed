using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdMedWeb.Models;
using AdMedWeb.Repository.IRepository;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Collections.Generic;
using AdMedWeb.Models.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdMedWeb.Controllers
{
    [Authorize]
    public class MedicationsController : Controller
    {

        private readonly IMedicationRepository _meRepo;
        private readonly IResidentRepository _reRepo;
        private readonly IEmailSender _emailSender;

        public MedicationsController(IMedicationRepository apRepo, IResidentRepository reRepo, IEmailSender emailSender)
        {
            _meRepo = apRepo;
            _reRepo = reRepo;
            _emailSender = emailSender;
        }

        public IActionResult Index(string residentId)
        {

            TempData["residentId"] = residentId;
            return View(new Medication() {});
        }

        [Authorize("Admin")]
        private async Task<Medication> Update(Medication obj, int? id)
        {
            obj = await GetUpdate(obj, id);

            return obj;
        }

        [Authorize(Roles = "Admin")]
        public async Task<Medication> GetUpdate(Medication obj, int? id)
        {

            // Flow will come here for Update
            obj = await _meRepo.GetAsync(SD.MedicationAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            return obj;

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(int? id)
        {

            IEnumerable<Resident> residentList = await _reRepo.GetAllAsync(SD.ResidentAPIPath, HttpContext.Session.GetString("JWToken"));

            MedicationViewModel objVM = new MedicationViewModel()
            {

                ResidentList = residentList.Select(i => new SelectListItem()
                {

                    Text = i.FirstName + " " + i.LastName,
                    Value = i.Id.ToString()

                }),
                Medication = new Medication()

            };

            if (id == null)
            {

                // This will be true for Insert / Create
                return View(objVM);
            }

            // Flow will come here for Update
            objVM.Medication = await _meRepo.GetAsync(SD.MedicationAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));

            if (objVM.Medication == null)
            {
                return NotFound();
            }

            return View(objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(MedicationViewModel obj)
        {

            if (ModelState.IsValid)
            {

                if (obj.Medication.Id == 0)
                {
                    await _meRepo.CreateAsync(SD.MedicationAPIPath, obj.Medication, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _meRepo.UpdateAsync(SD.MedicationAPIPath + obj.Medication.Id, obj.Medication, HttpContext.Session.GetString("JWToken"));
                }

                return RedirectToAction(nameof(Index));

            }

            IEnumerable<Resident> residentList = await _reRepo.GetAllAsync(SD.ResidentAPIPath, HttpContext.Session.GetString("JWToken"));

            MedicationViewModel objVM = new MedicationViewModel()
            {

                ResidentList = residentList.Select(i => new SelectListItem()
                {

                    Text = i.FirstName + " " + i.LastName,
                    Value = i.Id.ToString()

                }),
                Medication = new Medication()

            };

            return View(objVM);

        }

        [Authorize(Roles = "Admin")]
        public async Task PostUpdateMedication(Medication obj)
        {

            await _meRepo.UpdateAsync(SD.MedicationAPIPath + obj.Id, obj, HttpContext.Session.GetString("JWToken"));

        }

        public async Task<IActionResult> GetAllMedications()
        {

            var data = await _meRepo.GetAllAsync(SD.MedicationAPIPath, HttpContext.Session.GetString("JWToken"));

            IEnumerable <Medication> list = data.ToList();

            if (TempData["residentId"] != null)
            {
                list = list.Where(x => x.ResidentId.ToString().Equals(TempData["residentId"])).ToList();
            }

            return Json(new { data = list });

        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {

            var status = await _meRepo.DeleteAsync(SD.MedicationAPIPath, id, HttpContext.Session.GetString("JWToken"));

            if (status)
            {

                return Json(new { success = true, message = "Delete Successful" });

            }

            return Json(new { success = false, message = "Delete Not Successful" });

        }

    }

}
