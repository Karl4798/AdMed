using AdMedWeb.Models;
using AdMedWeb.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AdMedWeb.Controllers
{
    public class NewsController : Controller
    {
        private readonly IPostRepository _psRepo;

        public NewsController(IPostRepository apRepo)
        {
            _psRepo = apRepo;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View(await _psRepo.GetAllAsync(SD.PostAPIPath, HttpContext.Session.GetString("JWToken")));
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            return View(await _psRepo.GetAsync(SD.PostAPIPath, id, HttpContext.Session.GetString("JWToken")));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Upsert(int? id)
        {
            Post obj = new Post();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(Post obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    await _psRepo.CreateAsync(SD.PostAPIPath, obj, HttpContext.Session.GetString("JWToken"));
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    await UpdatePost(obj);
                }
            }
            return View(obj);
        }

        [Authorize(Roles = "Admin")]
        public async Task UpdatePost(Post obj)
        {
            await _psRepo.UpdateAsync(SD.PostAPIPath + obj.Id, obj, HttpContext.Session.GetString("JWToken"));
        }

        [Authorize("Admin")]
        private async Task<Post> Update(Post obj, int? id)
        {
            obj = await GetUpdate(obj, id);
            return obj;
        }

        [Authorize(Roles = "Admin")]
        public async Task<Post> GetUpdate(Post obj, int? id)
        {
            // Flow will come here for Update
            obj = await _psRepo.GetAsync(SD.PostAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            return obj;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            return View(await _psRepo.GetAsync(SD.PostAPIPath, id, HttpContext.Session.GetString("JWToken")));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Post obj)
        {
            var status = await _psRepo.DeleteAsync(SD.PostAPIPath, obj.Id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return RedirectToAction(nameof(Index));
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }
    }
}