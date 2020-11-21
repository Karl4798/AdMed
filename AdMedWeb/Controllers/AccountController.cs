using AdMedWeb.Models;
using AdMedWeb.Models.ViewModels;
using AdMedWeb.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static AdMedWeb.Models.Enums;

namespace AdMedWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IResidentRepository _reRepo;
        private readonly IAccountRepository _accRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(IResidentRepository reRepo, IAccountRepository accRepo, IHttpContextAccessor httpContextAccessor)
        {
            _reRepo = reRepo;
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
            IEnumerable<Resident> residents = await _reRepo.GetAllAsync(SD.ResidentAPIPath, HttpContext.Session.GetString("JWToken"));
            Resident resident = null;

            UpdateUserViewModel uuvm = null;

            foreach (var item in residents)
            {
                if (item.PrimaryContact.Email.Equals(user.Username))
                {
                    resident = item;
                }
            }

            if (user != null)
            {

                if (resident != null)
                {
                    uuvm = new UpdateUserViewModel()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Username = user.Username,
                        Role = user.Role,
                        RolesEnum = (Roles)Enum.Parse(typeof(Roles), user.Role),
                        ResidentId = resident.Id,
                        Resident = resident
                    };
                }
                else
                {
                    uuvm = new UpdateUserViewModel()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Username = user.Username,
                        Role = user.Role,
                        RolesEnum = (Roles)Enum.Parse(typeof(Roles), user.Role),
                        Resident = new Resident()
                    };
                }

                IEnumerable<Resident> residentList = await _reRepo.GetAllAsync(SD.ResidentAPIPath, HttpContext.Session.GetString("JWToken"));
                AccountViewModel objVM = new AccountViewModel()
                {
                    ResidentList = residentList.Select(i => new SelectListItem()
                    {
                        Text = i.FirstName + " " + i.LastName,
                        Value = i.Id.ToString()
                    }),
                    User = uuvm
                };
                // This will be true for Insert / Create
                return View(objVM);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpsertResidentAccount(AccountViewModel avm)
        {
            avm.User.Role = avm.User.RolesEnum.ToString();

            if (ModelState.IsValid)
            {
                User user = new User()
                {
                    Id = avm.User.Id,
                    FirstName = avm.User.FirstName,
                    LastName = avm.User.LastName,
                    Username = avm.User.Username,
                    Role = avm.User.RolesEnum.ToString()
                };

                Resident resident = await _reRepo.GetAsync(SD.ResidentAPIPath, avm.User.ResidentId, HttpContext.Session.GetString("JWToken"));
                resident.PrimaryContact.Email = avm.User.Username;

                await _reRepo.UpdateAsync(SD.ResidentAPIPath + avm.User.ResidentId, resident, HttpContext.Session.GetString("JWToken"));

                await _accRepo.UpdateAsync(SD.AccountAPIPath + user.Id, user, HttpContext.Session.GetString("JWToken"));

                return RedirectToAction(nameof(Index));
            }
            return View(avm);
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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AdminResetPassword(int? id)
        {
            AdminResetPasswordViewModel obj = new AdminResetPasswordViewModel();
            User user = await _accRepo.GetAsync(SD.AccountAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            obj.Username = user.Username;
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AdminResetPassword(AdminResetPasswordViewModel arpvm)
        {
            if (ModelState.IsValid)
            {
                await _accRepo.AdminResetPasswordAsync(SD.AccountAPIPath + "adminresetpassword/", arpvm, HttpContext.Session.GetString("JWToken"));
                return RedirectToAction(nameof(Upsert));
            }
            return View(arpvm);
        }

    }
}