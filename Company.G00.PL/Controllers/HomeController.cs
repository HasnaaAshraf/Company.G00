using System.Diagnostics;
using System.Text;
using Company.G00.PL.Models;
using Company.G00.PL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.G00.PL.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IScopedService scopedService01;
        private readonly IScopedService scopedService02;
        private readonly ITransentService transentService01;
        private readonly ITransentService transentService02;
        private readonly ISingletonService singletonService01;
        private readonly ISingletonService singletonService02;

        public HomeController(ILogger<HomeController> logger,
            IScopedService scopedService01,
            IScopedService scopedService02,
            ITransentService transentService01,
            ITransentService transentService02,
            ISingletonService singletonService01,
            ISingletonService singletonService02)
        {
            _logger = logger;
            this.scopedService01 = scopedService01;
            this.scopedService02 = scopedService02;
            this.transentService01 = transentService01;
            this.transentService02 = transentService02;
            this.singletonService01 = singletonService01;
            this.singletonService02 = singletonService02;
        }

        public string TestLifeStyle()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append($"scopedService01 :: {scopedService01.GetGuid()}\n");
            builder.Append($"scopedService02 :: {scopedService02.GetGuid()}\n");
            builder.Append($"transentService01 :: {transentService01.GetGuid()}\n\n");
            builder.Append($"transentService02 :: {transentService02.GetGuid()}\n\n");
            builder.Append($"singletonService01 :: {singletonService01.GetGuid()}\n");
            builder.Append($"singletonService02 :: {singletonService02.GetGuid()} \n");

            return builder.ToString();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
