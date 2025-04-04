﻿using Company.G00.DAL.Models;
using Company.G00.PL.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Company.G00.PL.Controllers
{
    public class UserController : Controller
    {

        // appsettings : MultipleActiveResultSets = true (Cause Error)


        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {

            IEnumerable<UserToReturnDto> users;
            if (string.IsNullOrEmpty(SearchInput))
            {
                users = _userManager.Users.AsNoTracking().Select(U => new UserToReturnDto()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles =  _userManager.GetRolesAsync(U).Result
                });
            }
            else
            {
                users = _userManager.Users.AsNoTracking().Select(U => new UserToReturnDto()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).Where(U => U.FirstName.ToLower().Contains(SearchInput.ToLower()));
            }

            return View(users);
            
        }


        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null) return BadRequest($" This Id = {id} InValid");

            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                return NotFound($"This Id {id} Not Found");
            }

            var dto = new UserToReturnDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user),
            };

            return View(viewName, dto);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id,"Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserToReturnDto model)
        {
            if (ModelState.IsValid)
            {
               if (id != model.Id) return BadRequest($" This Id = {id} InValid");

               var user = await _userManager.FindByIdAsync(id);

               if(user is null) return BadRequest(" InValid Operations ");

               user.UserName = model.UserName;
               user.FirstName = model.FirstName;
               user.LastName = model.LastName;
               user.Email = model.Email;

               var Result = await _userManager.UpdateAsync(user);

               if(Result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            ModelState.AddModelError("", "Errooor");

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string? id, UserToReturnDto model)
        {
            //if (ModelState.IsValid)
            //{
            if (id != model.Id) return BadRequest($" This Id = {id} InValid");

            var user = await _userManager.FindByIdAsync(id);

            if (user is null) return BadRequest(" InValid Operations ");

            
            var Result = await _userManager.DeleteAsync(user);

            if (Result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            //}
            return View(model);
        }


    }
}
