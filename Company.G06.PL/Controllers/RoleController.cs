using AutoMapper;
using Company.G06.DAL.Models;
using Company.G06.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Company.G06.PL.Controllers
{

    
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        

        public RoleController(RoleManager<IdentityRole> roleManager , UserManager<ApplicationUser> userManager )
        {
            _roleManager = roleManager;
           _userManager = userManager;
           
        }

        public async Task<IActionResult> Index(string SearchInput)
        {
            var roles = Enumerable.Empty<RoleViewModel>();
            if (string.IsNullOrEmpty(SearchInput))
            {
                roles = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name
                
                }).ToListAsync();

            }
            else
            {
                roles = await _roleManager.Roles.Where(R => R.Name.ToLower().Contains(SearchInput.ToLower())).Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name


                }).ToListAsync();

            }

            return View(roles);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (!ModelState.IsValid)
            {

                var Role = new IdentityRole()
                {

                    Name = model.RoleName
                };

                var result  = await _roleManager.CreateAsync(Role);
                if(result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));

                }

                
            }


            
         
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null) return BadRequest();

            var rolesFromDb = await _roleManager.FindByIdAsync(id);

            if (rolesFromDb == null)
                return NotFound();


            var role = new RoleViewModel()
            {
                Id = rolesFromDb.Id,
                RoleName = rolesFromDb.Name


            };


            return View(viewName, role);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? Id)
        {
            //if (id is null) return BadRequest();

            //var department = _departmentRepository.Get(id.Value);

            //if (department is null) return NotFound();

            //return View(department);



            return await Details(Id, "Edit");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string? Id, RoleViewModel model)
        {
            try
            {
                if (Id != model.Id) return BadRequest();
                if (ModelState.IsValid)
                {



                    var rolesFromDb = await _roleManager.FindByIdAsync(Id);

                    if (rolesFromDb == null)
                        return NotFound();


                    rolesFromDb.Name = model.RoleName;

                    var result = await _roleManager.UpdateAsync(rolesFromDb);

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
        public async Task<IActionResult> Delete([FromRoute] string? Id, RoleViewModel model)
        {
            try
            {
                if (Id != model.Id) return BadRequest();
                if (ModelState.IsValid)
                {
                    var rolesFromDb = await _roleManager.FindByIdAsync(Id);

                    if (rolesFromDb == null)
                        return NotFound();

                    var result = await _roleManager.DeleteAsync(rolesFromDb);

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
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if(role == null)
                return NotFound();

            ViewData["RoleId"] = roleId;

            var usersInRole = new List<UsersInRoleViewModel>();
            var users  = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userInRole = new UsersInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName

                };

                if(await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;   
                }
                else
                {
                    userInRole.IsSelected= false;

                }

                usersInRole.Add(userInRole);    
                
            }

            return View(usersInRole);

        }


        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId , List<UsersInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return NotFound();

            if(ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if(appUser is not null)
                    {
                        if(user.IsSelected && ! await _userManager.IsInRoleAsync(appUser , role.Name))
                        {
                            await _userManager.AddToRoleAsync(appUser , role.Name); 

                        }
                        else if(!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                        }
                    }
                    
                }

                return RedirectToAction(nameof(Edit), new { id = roleId } );

            }

            return View(users);

        }

    }
}
