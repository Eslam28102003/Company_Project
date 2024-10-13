using Company.G06.BLL.Interfaces;
using Company.G06.BLL.Repositories;
using Company.G06.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.G06.PL.Controllers
{
	[Authorize]
	public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(/*IDepartmentRepository departmentRepository*/ IUnitOfWork unitOfWork) // ASK CLR To Create Oblect From DepartmentRepository
        {
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(departments);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]


        public async Task<IActionResult> Create(Department model)
        {
            if(ModelState.IsValid)
            {
                 _unitOfWork.DepartmentRepository.AddAsync(model);
                var Count =  _unitOfWork.Complete();
                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(model);
            
        }
        public  async Task<IActionResult> Details(int? id , string viewName = "Details")
        {
            if (id is null) return BadRequest();

            var department =  await _unitOfWork.DepartmentRepository.GetAsync(id.Value);

            if (department is null) return NotFound();

            return View(viewName , department);

        }


        [HttpGet]
        public  async Task<IActionResult> Edit(int? id)
        {
            //if (id is null) return BadRequest();

            //var department = _departmentRepository.Get(id.Value);

            //if (department is null) return NotFound();

            //return View(department);

            return await  Details(id, "Edit");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<IActionResult> Edit([FromRoute]int? Id ,Department department)
        {
            try
            {
                if (Id != department.Id) return BadRequest();
                if (ModelState.IsValid)
                {
                     _unitOfWork.DepartmentRepository.UpdateAsync(department);
                    var count = _unitOfWork.Complete();
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));


                    }

                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
           

         
            return  View(department);

        }

        [HttpGet]

        public async Task<IActionResult> Delete(int? id)
        {
            //if (id is null) return BadRequest();
            //var department = _departmentRepository.Get(id.Value);

            //if (department is null) return NotFound();

            //return View(department);
            return await Details(id, "Delete");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int? Id, Department department)
        {
            try
            {
                if (Id != department.Id) return BadRequest();
                if (ModelState.IsValid)
                {
                    _unitOfWork.DepartmentRepository.DeleteAsync(department);
                    var count = _unitOfWork.Complete();
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));


                    }

                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }



            return View(department);

        }




    }
}
