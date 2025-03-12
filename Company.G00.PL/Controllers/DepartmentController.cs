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


        [HttpGet]
        public IActionResult Details(int id)
        {
            var departmentDetails = _departmentRepository.Get(id);

            if(departmentDetails == null)
            {
                ViewBag.Message = "Department Details Not Found";
                return View("Not Found Data");
            }

            var dto = new DetailsDepartmentDto
            {
                Code = departmentDetails.Code,
                Name = departmentDetails.Name,
                CreateAt = departmentDetails.CreateAt
            };

            return View(dto);

        }

        // Another Solution For Details

        //[HttpGet]
        //public IActionResult Details(int id)
        //{
        //    if (id == null) BadRequest("Invalid Id"); // 400

        //    var departmentDetails = _departmentRepository.Get(id);

        //    if (departmentDetails == null)
        //    {
        //        return NotFound(new { StatusCode = 404, message = "Not Found Data" }); // Built In Not Found Method 
        //    }

        //    return View(departmentDetails);

        //}



        [HttpGet]
        public IActionResult Edit(int id)
        {
            var department = _departmentRepository.Get(id);

            if (department == null)
            {
                return NotFound("Not Found Data");
            }

             var dto = new UpdateDepartmentDto()
             {
                Code = department.Code,
                Name = department.Name,
                CreateAt = department.CreateAt
             };


               return View(dto);
        }

        [HttpPost]
        public IActionResult Edit(UpdateDepartmentDto model)
        {
            if (ModelState.IsValid) // Server Side Validation 
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };

                var count = _departmentRepository.Update(department);

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

            }

            return View(model);
        }



    }

}
