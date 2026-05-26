using Microsoft.AspNetCore.Mvc;
using UHabitacional.MVC.Services;
using UHabitacional.MVC.ViewModels;

namespace UHabitacional.MVC.Controllers;

[AuthorizeRole]
public class HomeController : Controller
{
    private readonly ICurrentUser _current;

    public HomeController(ICurrentUser current) => _current = current;

    public IActionResult Index()
    {
        ViewBag.PageTitle = "Inicio";
        ViewBag.Breadcrumbs = new List<BreadcrumbItem>
        {
            new() { Label = "Home" }
        };
        return View();
    }

    public IActionResult Error() => View();
}
