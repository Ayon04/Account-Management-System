using JobTask.Models;
using JobTask.Services;
using Microsoft.AspNetCore.Mvc;

public class CAController : Controller
{
    private readonly CAService _caService;

    public CAController(CAService caService)
    {
        _caService = caService;
    }

    [HttpGet]
    [Route("ManageCA")]
    public IActionResult ManageCA()
    {
        var accounts = _caService.GetAllAccounts();
        return View("ManageCA", accounts);
    }

    [HttpGet]
    [Route("ViewCA")]
    public IActionResult ViewCA()
    {
        var accounts = _caService.GetAllAccounts();
        return View("ViewCA", accounts);
    }

    [HttpPost]
    public IActionResult Create(CA ca)
    {
        _caService.CreateAccount(ca);
        return RedirectToAction("ManageCA");
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        _caService.DeleteAccount(id);
        return RedirectToAction("ManageCA");
    }
}
