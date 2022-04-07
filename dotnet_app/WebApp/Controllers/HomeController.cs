using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers;

public class HomeController : Controller
{
    readonly IValorantService _valorantService;

    public HomeController(IValorantService valorantService)
    {
        _valorantService = valorantService;
    }

    public async Task<IActionResult> Index()
    {
        var data = await _valorantService.GetSkins();

        return View(data);
    }

    [Route("boljomir")]
    public IActionResult Boljomir()
    {
        return View();
    }
}
