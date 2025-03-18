using Company.G00.BLL.Interfaces;
using Company.G00.BLL.Repositories;
using Company.G00.DAL.Models;
using Company.G00.PL.Dtos;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace Company.G00.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        // Ask CLR To Create Object 
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository= departmentRepository; // This For Make CLR Make Object Not You To Handle Everything Without new
        }

        [HttpGet] // Department/Index
        public IActionResult Index()
        {
            var department = _departmentRepository.GetAll();
            return View(department); // Send Department As Model llView
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid) // Server Side Validation 
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };

                var count = _departmentRepository.Add(department);

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

            }

            return View(model);
        }


        //[HttpGet]
        //public IActionResult Details(int id)
        //{
        //    var departmentDetails = _departmentRepository.Get(id);

        //    if(departmentDetails == null)
        //    {
        //        ViewBag.Message = "Department Details Not Found";
        //        return View("Not Found Data");
        //    }

        //    var dto = new DetailsDepartmentDto
        //    {
        //        Code = departmentDetails.Code,
        //        Name = departmentDetails.Name,
        //        CreateAt = departmentDetails.CreateAt
        //    };

        //    return View(dto);

        //}

        // Another Solution For Details

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null) BadRequest("Invalid Id"); // 400

            var departmentDetails = _departmentRepository.Get(id.Value);

            if (departmentDetails is null)
            {
                return NotFound(new { StatusCode = 404, message = "Not Found Data" }); // Built In Not Found Method 
            }

            return View(viewName, departmentDetails);

        }



        [HttpGet]
        public IActionResult Edit(int? id )
        {
            if (id is null) BadRequest("Invalid Id"); // 400

            var departmentDetails = _departmentRepository.Get(id.Value);

            if (departmentDetails is null)
            {
                return NotFound(new { StatusCode = 404, message = "Not Found Data" }); // Built In Not Found Method 
            }

            var dto = new CreateDepartmentDto()
            {
                Code = departmentDetails.Code,
                Name = departmentDetails.Name,
                CreateAt = departmentDetails.CreateAt
            };

            return View(dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {

                //if (id != department.Id) return BadRequest($" This Id = {id} InValid");

                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };

                var Count = _departmentRepository.Update(department);
                if (Count > 0)
                {
                   return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int id, UpdateDepartmentDto department)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        //if (id is null) return BadRequest($" This Id = {id} InValid");

        //        var departmentUpdate = new Department()
        //        {
        //            Id = id,
        //            Code = department.Code,
        //            Name= department.Name,
        //            CreateAt = department.CreateAt
        //        };

        //        var Count = _departmentRepository.Update(departmentUpdate);
        //        if (Count > 0)
        //        {
        //            RedirectToAction("Index");
        //        }
        //    }
        //    return View(department);
        //}


        [HttpGet]
        public IActionResult Delete(int? id)
        {
        
            return Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int? id, Department department)
        {
            if (ModelState.IsValid)
            {
                if (id is null) return BadRequest($" This Id = {id} InValid");

                var Count = _departmentRepository.Delete(department);
                if (Count > 0)
                {
                   return RedirectToAction("Index");
                }
            }
            return View(department);
        }

    }

}
