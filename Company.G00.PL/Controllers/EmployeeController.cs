﻿using System.Collections;
using AutoMapper;
using Company.G00.BLL.Interfaces;
using Company.G00.DAL.Models;
using Company.G00.PL.Dtos;
using Company.G00.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Company.G00.PL.Controllers
{

    [Authorize]
    public class EmployeeController : Controller
    {

        //private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        // Ask CLR To Make Object From IEmployeeRepository
        public EmployeeController(
          //  IEmployeeRepository employeeRepository
          //, IDepartmentRepository departmentRepository,
             IUnitOfWork unitOfWork,
             IMapper mapper)
        {
            //_employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
            //_departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employee;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employee = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }else
            {
                employee = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
            }
            
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
        public async Task<IActionResult> RealSearch(string? SearchInput)
        {
            IEnumerable<Employee> employee;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employee = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                employee = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
            }

            return PartialView("EmployeePartialView/EmployeeRealTimeSearchPartialView", employee);
        }


        [HttpGet]
        public IActionResult Create()
        {
            //var department = _departmentRepository.GetAll();
            //ViewData["department"] = department;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto create)
        {
            if (ModelState.IsValid)
            {
                // Manual Mapping
                //var employee = new Employee()
                //{
                //    Name = create.Name,
                //    Age = create.Age,
                //    Address = create.Address,
                //    Email = create.Email,
                //    Phone = create.Phone,
                //    Salary = create.Salary,
                //    IsActive = create.IsActive,
                //    IsDeleted = create.IsDeleted,
                //    HiringDate = create.HiringDate,
                //    CreateAt = create.CreateAt,
                //    DepartmentId = create.DepartmentId
                //};

                
                if(create.Image is not null)
                {
                   create.ImageName =  DocumentSettings.UploadFile(create.Image, "images");
                }

                var employee = _mapper.Map<Employee>(create);

                await _unitOfWork.EmployeeRepository.AddAsync(employee);

                var Count = await _unitOfWork.CompleteAsync();

                if (Count > 0)
                {
                    TempData["Message"] = "Employee Is Created ";
                  return RedirectToAction("Index");
                }
            }
            return View(create);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest($" This Id = {id} InValid");

            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);

            if (employee is null)
            {
                return NotFound($"This Id {id} Not Found");
            }
            return View(viewName,employee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //var department = _departmentRepository.GetAll();
            //ViewData["department"] = department;
            if (id is null) return BadRequest($" This Id = {id} InValid");

            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);

            if (employee is null)
            {
                return NotFound($"This Id {id} Not Found");
            }

            //var employeeDto = new CreateEmployeeDto()
            //{
            //    Name = employee.Name,
            //    Age = employee.Age,
            //    Address = employee.Address,
            //    Email = employee.Email,
            //    Phone = employee.Phone,
            //    Salary = employee.Salary,
            //    IsActive = employee.IsActive,
            //    IsDeleted = employee.IsDeleted,
            //    HiringDate = employee.HiringDate,
            //    CreateAt = employee.CreateAt
            //};

            var employeeDto = _mapper.Map<CreateEmployeeDto>(employee);

            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                //if (id is null) return BadRequest($" This Id = {id} InValid");


                var employee = await _unitOfWork.EmployeeRepository.GetAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }

                if (model.ImageName is not null && model.Image is not null)
                {
                    DocumentSettings.Delete(model.ImageName, "images");
                }

                if (model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }

                employee.Name = model.Name;
                employee.Age = model.Age;
                employee.Address = model.Address;
                employee.Email = model.Email;
                employee.Phone = model.Phone;
                employee.Salary = model.Salary;
                employee.IsActive = model.IsActive;
                employee.IsDeleted = model.IsDeleted;
                employee.HiringDate = model.HiringDate;
                employee.CreateAt = model.CreateAt;
                employee.DepartmentId = model.DepartmentId;

                _unitOfWork.EmployeeRepository.Update(employee);

                var Count = await _unitOfWork.CompleteAsync();

                if (Count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit([FromRoute] int id, CreateEmployeeDto model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // ✅ 1️⃣ جلب الموظف القديم من الداتا بيز
        //    var employee = await _unitOfWork.EmployeeRepository.GetAsync(id);
        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }

        //    // ✅ 2️⃣ تحديث الصورة فقط لو المستخدم رفع صورة جديدة
        //    if (model.Image != null)
        //    {
        //        // حذف الصورة القديمة لو موجودة
        //        if (!string.IsNullOrEmpty(employee.ImageName))
        //        {
        //            DocumentSettings.Delete(employee.ImageName, "images");
        //        }

        //        // رفع الصورة الجديدة **وحفظ اسمها في الداتا بيز**
        //        employee.ImageName = DocumentSettings.UploadFile(model.Image, "images");
        //    }

        //    // ✅ 3️⃣ تحديث باقي بيانات الموظف
        //    employee.Name = model.Name;
        //    employee.Age = model.Age;
        //    employee.Address = model.Address;
        //    employee.Email = model.Email;
        //    employee.Phone = model.Phone;
        //    employee.Salary = model.Salary;
        //    employee.IsActive = model.IsActive;
        //    employee.IsDeleted = model.IsDeleted;
        //    employee.HiringDate = model.HiringDate;
        //    employee.CreateAt = model.CreateAt;
        //    employee.DepartmentId = model.DepartmentId;

        //    // ✅ 4️⃣ حفظ التعديلات في الداتا بيز
        //    _unitOfWork.EmployeeRepository.Update(employee);
        //    var count = await _unitOfWork.CompleteAsync();

        //    if (count > 0)
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    return View(model);
        //}


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int? id, Employee employee)
        {
            //if (ModelState.IsValid)
            //{
                if (id is null) return BadRequest($" This Id = {id} InValid");

            var departmentDelete = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            _unitOfWork.EmployeeRepository.Delete(departmentDelete);

            var Count = await _unitOfWork.CompleteAsync();

                if (Count > 0)
                {
                   if(employee.ImageName is not null)
                   {
                      DocumentSettings.Delete(employee.ImageName,"images");
                   }
                   return RedirectToAction("Index");
                }
            //}
            return View(employee);
        }

    }
}
