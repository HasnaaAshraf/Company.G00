using Company.G00.BLL.Interfaces;
using Company.G00.BLL.Repositories;
using Microsoft.AspNetCore.Mvc;

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
    }
}
