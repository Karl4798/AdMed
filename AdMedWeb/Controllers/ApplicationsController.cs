using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdMedWeb.Models;
using AdMedWeb.Repository.IRepository;
using Newtonsoft.Json;

namespace AdMedWeb.Controllers
{
    [Authorize]
    public class ApplicationsController : Controller
    {

        private readonly IApplicationRepository _apRepo;

        public ApplicationsController(IApplicationRepository apRepo)
        {
            _apRepo = apRepo;
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

            obj = await GetUpdate(obj, id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
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
                    TempData.Put<Application>("Application", obj);
                    return RedirectToAction("Upsert", "EmergencyContacts");
                    //await _apRepo.CreateAsync(SD.ApplicationAPIPath, obj, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await PostUpdate(obj);
                    
                }

                return RedirectToAction(nameof(Index));

            }

            return View(obj);

        }

        [Authorize(Roles = "Admin")]
        public async Task PostUpdate(Application obj)
        {

            await _apRepo.UpdateAsync(SD.ApplicationAPIPath + obj.Id, obj, HttpContext.Session.GetString("JWToken"));

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
