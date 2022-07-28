using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Readbuk.Admin.Attributes;
using Readbuk.Admin.Models;

namespace Readbuk.Admin.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (String.IsNullOrEmpty(HttpContext.Session.GetString("token"))) return RedirectToAction("Index", "Auth");
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

