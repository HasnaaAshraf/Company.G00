using Company.G00.BLL.Interfaces;
using Company.G00.DAL.Models;
using Company.G00.PL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Company.G00.PL.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly IGenericRepository<Employee> _employeeRepository;

        public EmployeeController(IGenericRepository<Employee> employeeRepository)
        {
            this._employeeRepository = employeeRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var employee = _employeeRepository.GetAll();
            return View(employee);
        }

        [HttpGet]
        public IActionResult Create()
        {
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
                    Phone = create.Phone,
                    Salary = create.Salary,
                    IsActive = create.IsActive,
                    IsDeleted = create.IsDeleted,
                    HiringDate = create.HiringDate,
                    CreateAt = create.CreateAt
                };

                var Count = _employeeRepository.Add(employee);
                if (Count > 0)
                {
                    RedirectToAction("Index");
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
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int? id , Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (id is null) return BadRequest($" This Id = {id} InValid");

                var Count = _employeeRepository.Update(employee);
                if (Count > 0)
                {
                    RedirectToAction("Index");
                }
            }
            return View(employee);
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
                    RedirectToAction("Index");
                }
            }
            return View(employee);
        }

    }
}
