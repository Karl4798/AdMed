using AdMedWeb.Models;
using AdMedWeb.Models.ViewModels;
using AdMedWeb.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using static AdMedWeb.Models.Enums;

namespace AdMedWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(IAccountRepository accRepo, IHttpContextAccessor httpContextAccessor)
        {
            _accRepo = accRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View(new User() { });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAccounts()
        {
            return Json(new { data = await _accRepo.GetAllAsync(SD.AccountAPIPath, HttpContext.Session.GetString("JWToken"))});
        }

        [Authorize(Roles = "Admin,Resident")]
        public async Task<ActionResult> Upsert()
        {
            var id = User.FindFirstValue(ClaimTypes.Sid);
            User user = await _accRepo.GetAsync(SD.AccountAPIPath, id, HttpContext.Session.GetString("JWToken"));
            if (user != null)
            {
                UpdateUserViewModel uuvm = new UpdateUserViewModel()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.Username,
                    Role = user.Role
                };
                return View(uuvm);
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpsertResidentAccount(int? id)
        {

            User user = await _accRepo.GetAsync(SD.AccountAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));

            if (user != null)
            {
                UpdateUserViewModel uuvm = new UpdateUserViewModel()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.Username,
                    RolesEnum = (Roles)Enum.Parse(typeof(Roles), user.Role)
                };
                return View(uuvm);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpsertResidentAccount(UpdateUserViewModel uuvm)
        {

            
            uuvm.Role = uuvm.RolesEnum.ToString();
            Debug.WriteLine(uuvm);

            if (ModelState.IsValid)
            {
                User user = new User()
                {
                    Id = uuvm.Id,
                    FirstName = uuvm.FirstName,
                    LastName = uuvm.LastName,
                    Username = uuvm.Username,
                    Role = uuvm.RolesEnum.ToString()
                };
                await _accRepo.UpdateAsync(SD.AccountAPIPath + user.Id, user, HttpContext.Session.GetString("JWToken"));
                return RedirectToAction(nameof(Index));
            }
            return View(uuvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Resident")]
        public async Task<ActionResult> Upsert(UpdateUserViewModel uuvm)
        {
            if (ModelState.IsValid)
            {
                User user = new User()
                {
                    Id = uuvm.Id,
                    FirstName = uuvm.FirstName,
                    LastName = uuvm.LastName,
                    Username = uuvm.Username,
                    Role = uuvm.Role
                };
                await _accRepo.UpdateAsync(SD.AccountAPIPath + user.Id, user, HttpContext.Session.GetString("JWToken"));
                var username = User.FindFirstValue(ClaimTypes.Name);
                if (!user.Username.Equals(username))
                {
                    return RedirectToAction("Logout", "Authentication");
                }
                return View(uuvm);
            }
            return View(uuvm);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Resident")]
        public async Task<ActionResult> ResetPassword()
        {
            ResetPasswordViewModel obj = new ResetPasswordViewModel();
            var username = User.FindFirstValue(ClaimTypes.Name);
            obj.Username = username;
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Resident")]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel rpvm)
        {
            if (ModelState.IsValid)
            {
                await _accRepo.ResetPasswordAsync(SD.AccountAPIPath + "resetpassword/", rpvm, HttpContext.Session.GetString("JWToken"));
                return RedirectToAction(nameof(Upsert));
            }
            return View(rpvm);
        }
    }
}