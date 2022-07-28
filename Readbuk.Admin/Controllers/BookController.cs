using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Readbuk.Admin.Attributes;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Readbuk.Admin.Controllers
{
    [AuthorizationFilter]
    public class BookController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public BookController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}

