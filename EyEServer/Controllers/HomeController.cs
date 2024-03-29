﻿using EyEServer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
namespace EyEServer.Controllers;

[Route("api/[controller]/[action]")]
public class HomeController : Controller
{
    public HomeController()
    {
    }
    //[Route("~/", Name = "default")]
    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
