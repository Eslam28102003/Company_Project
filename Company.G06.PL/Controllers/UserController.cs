using Company.G06.DAL.Models;
using Company.G06.PL.Helpers;
using Company.G06.PL.ViewModels;
using Company.G06.PL.ViewModels.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.G06.PL.Controllers
{

   
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string SearchInput)
		{
			var users = Enumerable.Empty<UserViewModel>();
			if (string.IsNullOrEmpty(SearchInput))
			{
				users = await _userManager.Users.Select(U => new UserViewModel()
				{
					Id = U.Id,
					FirstName = U.FirstName,
					LastName = U.LastName,
					Email = U.Email,
					Roles = _userManager.GetRolesAsync(U).Result
				}).ToListAsync();
					
				

			}
			else
			{
				users = await _userManager.Users.Where(U => U.Email.ToLower().Contains(SearchInput.ToLower())).Select(U => new UserViewModel()
				{
					Id = U.Id,
					FirstName = U.FirstName,
					LastName = U.LastName,
					Email = U.Email,
					Roles = _userManager.GetRolesAsync(U).Result


				}).ToListAsync();
				
			}

			



		
			return View(users);
		}

        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null) return BadRequest();

            var userFromDb = await _userManager.FindByIdAsync(id);

			if (userFromDb == null) 
				return NotFound();


			var user = new UserViewModel()
			{
				Id = userFromDb.Id,
				FirstName = userFromDb.FirstName,
				LastName = userFromDb.LastName,
				Email = userFromDb.Email,
				Roles = _userManager.GetRolesAsync(userFromDb).Result

			};


            return View(viewName, user);

        }

        public async Task<IActionResult> Edit(string? id)
        {
            //if (id is null) return BadRequest();

            //var department = _departmentRepository.Get(id.Value);

            //if (department is null) return NotFound();

            //return View(department);



            return await Details(id, "Edit");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string? Id, UserViewModel model)
        {
            try
            {
                if (Id != model.Id) return BadRequest();
                if (ModelState.IsValid)
                {


                    var userFromDb = await _userManager.FindByIdAsync(Id);

                    if (userFromDb == null)
                        return NotFound();


                   userFromDb.FirstName = model.FirstName;
                   userFromDb.LastName = model.LastName;
                   userFromDb.Email = model.Email;

                   var result =  await _userManager.UpdateAsync(userFromDb);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));

                    }
                       


                    

                }




            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }





            return View(model);

        }

        [HttpGet]

        public async Task<IActionResult> Delete(string? id)
        {
            //if (id is null) return BadRequest();
            //var department = _departmentRepository.Get(id.Value);

            //if (department is null) return NotFound();

            //return View(department);


            return await Details(id, "Delete");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string? Id, UserViewModel model)
        {
            try
            {
                if (Id != model.Id) return BadRequest();
                if (ModelState.IsValid)
                {
                    var userFromDb = await _userManager.FindByIdAsync(Id);

                    if (userFromDb == null)
                        return NotFound();
                   
                    var result = await _userManager.DeleteAsync(userFromDb);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));

                    }

                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }



            return View(model);

        }
    }
}
