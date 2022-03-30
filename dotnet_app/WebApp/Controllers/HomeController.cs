using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers;

public class HomeController : Controller
{
    readonly ValorantService _valorantService;

    public HomeController(ValorantService valorantService)
    {
        _valorantService = valorantService;
    }

    public async Task<IActionResult> Index()
    {
        var data = await _valorantService.GetSkins();

        return View(data);
    }
}
