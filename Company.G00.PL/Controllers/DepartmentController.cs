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
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        // Ask CLR To Create Object 
        public DepartmentController(/*IDepartmentRepository departmentRepository*/ IUnitOfWork unitOfWork)
        {
            //_departmentRepository= departmentRepository; // This For Make CLR Make Object Not You To Handle Everything Without new
            _unitOfWork = unitOfWork;
        }

        [HttpGet] // Department/Index
        public async Task<IActionResult> Index()
        {
            var department = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(department); // Send Department As Model llView
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid) // Server Side Validation 
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };

                await _unitOfWork.DepartmentRepository.AddAsync(department);

                var Count = await _unitOfWork.CompleteAsync();

                if (Count > 0)
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
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null) BadRequest("Invalid Id"); // 400

            var departmentDetails = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);

            if (departmentDetails is null)
            {
                return NotFound(new { StatusCode = 404, message = "Not Found Data" }); // Built In Not Found Method 
            }

            return View(viewName, departmentDetails);

        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id )
        {
            if (id is null) BadRequest("Invalid Id"); // 400

            var departmentDetails = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);

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
        public async Task<IActionResult> Edit([FromRoute] int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {

                //if (id != department.Id) return BadRequest($" This Id = {id} InValid");

                var department = new Department()
                {
                    Id = id,
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };

                 _unitOfWork.DepartmentRepository.Update(department);

                var Count = await _unitOfWork.CompleteAsync();

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
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int? id, Department department)
        {
            //if (ModelState.IsValid)
            //{
                if (id is null) return BadRequest($" This Id = {id} InValid");

               
                var departmentDelete = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
                _unitOfWork.DepartmentRepository.Delete(departmentDelete);

                var Count = await _unitOfWork.CompleteAsync();

                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            //}
            return View(department);
        }

    }

}
