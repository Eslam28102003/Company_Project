using AutoMapper;
using Company.G06.BLL.Interfaces;
using Company.G06.DAL.Models;
using Company.G06.PL.Helpers;
using Company.G06.PL.ViewModels.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace Company.G06.PL.Controllers
{
	[Authorize]
	public class EmployeesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private  IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeesController(/*IEmployeeRepository employeeRepository , IDepartmentRepository departmentRepository*/
            IUnitOfWork unitOfWork,
            
            IMapper mapper) // ASK CLR To Create Oblect From DepartmentRepository
        {
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

       
        public  async Task<IActionResult> Index(string SearchInput)
        {
            var employee = Enumerable.Empty<Employee>();
            if(string.IsNullOrEmpty(SearchInput))
            {
                employee = await _unitOfWork.EmployeeRepository.GetAllAsync();

            }
            else
            {
                employee = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
            }

            var Result = _mapper.Map<IEnumerable<EmployeeViewModel>>(employee);



            //string Message = "Hello World";

            //// View's Dictionary : [Extra Information]Transfer Data From Action To View [One way] Required Casting
            //// 1.ViewData  : Property Inherited From Controller : Dictionary

            //ViewData["Message"] = Message + " From ViewData";
            //// 2.ViewBag   : Property Inherited From Controller : Dynamic [Not Required Casting]

            //ViewBag.Message = Message + " From ViewBag";
            //// 3.TempData  : Property Inherited From Controller : Dictionary
            /// Transfer For The Data From Request To Anothor

            //TempData["Message"] = Message + " From TempData";

            return View(Result);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {

            var departments =await _unitOfWork.DepartmentRepository.GetAllAsync();

            ViewData["Departments"] = departments;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]


        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {

                model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                // Casting EmployeeViewModel(ViewModel)--->Employee(Model)
                // Mapping
                // 1.Manual Mapping
                //Employee employee = new Employee()
                //{ 
                //    Id = model.Id,
                //    Name = model.Name,
                //    Address = model.Address,
                //    Email = model.Email,
                //    PhoneNumber = model.PhoneNumber,
                //    IsActive = model.IsActive,
                //    Age = model.Age,
                //    HiringDate = model.HiringDate,
                //    WorkFor = model.WorkFor,
                //    WorkForId = model.WorkForId,
                //    Salary = model.Salary




                //};

                // 2.Auto Mapping

                var employee = _mapper.Map<Employee>(model);



                _unitOfWork.EmployeeRepository.AddAsync(employee);
                var count = _unitOfWork.Complete();

                if (count > 0)
                {
                    TempData["Message"] = " Employee Is Created Successfuly";
                }
                else
                {
                    TempData["Message"] = " Employee Is Not Created Successfuly";


                }

                return RedirectToAction(nameof(Index));


            }
            return View(model);

        }
       


        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest();

            var employee =  await _unitOfWork.EmployeeRepository.GetAsync(id.Value);

            if (employee is null) return NotFound();

            var departments =  await _unitOfWork.DepartmentRepository.GetAllAsync();

            ViewData["Departments"] = departments;

            var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);


            return View(viewName, employeeViewModel);

        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id is null) return BadRequest();

            //var department = _departmentRepository.Get(id.Value);

            //if (department is null) return NotFound();

            //return View(department);

           

            return await Details(id, "Edit");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<IActionResult> Edit([FromRoute] int? Id, EmployeeViewModel model)
        {
            try
            {
                if (Id != model.Id) return BadRequest();
                if (ModelState.IsValid)
                {
                    if(model.ImageName is not null)
                    {
                        DocumentSettings.DeleteFile(model.ImageName, "images");
                    }
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                    var employee = _mapper.Map<Employee>(model);



                    _unitOfWork.EmployeeRepository.UpdateAsync(employee);
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

           



            return View(model);

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
        public async Task<IActionResult> Delete([FromRoute] int? Id, EmployeeViewModel model)
        {
            try
            {
                if (Id != model.Id) return BadRequest();
                if (ModelState.IsValid)
                {
                    var employee = _mapper.Map<Employee>(model);


                    _unitOfWork.EmployeeRepository.DeleteAsync(employee);
                    var count = _unitOfWork.Complete();
                    if (count > 0)

                    {
                        DocumentSettings.DeleteFile(model.ImageName, "images");
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
