using Company.G00.BLL.Interfaces;
using Company.G00.DAL.Models;
using Company.G00.PL.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Company.G00.PL.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;

        // Ask CLR To Make Object From IEmployeeRepository
        public EmployeeController(IEmployeeRepository employeeRepository , IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var employee = _employeeRepository.GetAll();
            // Dictionary : 3 Prop 
            // View Data : Transfer Extra Data From Controller (Action ) To View

            //ViewData["Message"] = "Hello From View Data";

            // View Bag  : Transfer Extra Data From Controller (Action ) To View

            //ViewBag.Message = "Hello From View Bag";
            //ViewBag.Message = new { "Hello From View Bag" };

            // Temp Data .
            return View(employee);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var department = _departmentRepository.GetAll();
            ViewData["department"] = department;
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto create)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee()
                {
                    Name = create.Name,
                    Age = create.Age,
                    Address = create.Address,
                    Email = create.Email,
                    Phone = create.Phone,
                    Salary = create.Salary,
                    IsActive = create.IsActive,
                    IsDeleted = create.IsDeleted,
                    HiringDate = create.HiringDate,
                    CreateAt = create.CreateAt,
                    DepartmentId = create.DepartmentId
                };

                var Count = _employeeRepository.Add(employee);

                if (Count > 0)
                {
                    TempData["Message"] = "Employee Is Created ";
                  return RedirectToAction("Index");
                }
            }
            return View(create);
        }

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest($" This Id = {id} InValid");

            var employee = _employeeRepository.Get(id.Value);

            if (employee is null)
            {
                return NotFound($"This Id {id} Not Found");
            }
            return View(viewName,employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var department = _departmentRepository.GetAll();
            ViewData["department"] = department;
            if (id is null) return BadRequest($" This Id = {id} InValid");

            var employee = _employeeRepository.Get(id.Value);

            if (employee is null)
            {
                return NotFound($"This Id {id} Not Found");
            }

            var employeeDto = new CreateEmployeeDto()
            {
                Name = employee.Name,
                Age = employee.Age,
                Address = employee.Address,
                Email = employee.Email,
                Phone = employee.Phone,
                Salary = employee.Salary,
                IsActive = employee.IsActive,
                IsDeleted = employee.IsDeleted,
                HiringDate = employee.HiringDate,
                CreateAt = employee.CreateAt
            };

            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id , CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                //if (id is null) return BadRequest($" This Id = {id} InValid");

                var employee = new Employee()
                {
                    Id = id,
                    Name = model.Name,
                    Age = model.Age,
                    Address = model.Address,
                    Email = model.Email,
                    Phone = model.Phone,
                    Salary = model.Salary,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    HiringDate = model.HiringDate,
                    CreateAt = model.CreateAt,
                    DepartmentId = model.DepartmentId
                };

                var Count = _employeeRepository.Update(employee);

                if (Count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int? id, Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (id is null) return BadRequest($" This Id = {id} InValid");

                var Count = _employeeRepository.Delete(employee);
                if (Count > 0)
                {
                   return RedirectToAction("Index");
                }
            }
            return View(employee);
        }

    }
}
