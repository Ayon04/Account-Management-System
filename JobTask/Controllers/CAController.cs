using JobTask.Models;
using JobTask.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

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
    [Route("CreateCA")]
    public IActionResult CreateCA()
    {
        return View("CreateCA");
    }

    [HttpPost]
    [Route("CreateCA")]
    public IActionResult CreateCA(CA ca)
    {
        if (!ModelState.IsValid)
            return View(ca);

        try
        {
            _caService.CreateAccount(ca);
            return RedirectToAction("ManageCA");
        }
        catch (SqlException ex)
        {
          
            if (ex.Number == 2627 || ex.Number == 2601) 
            {
                ModelState.AddModelError("Number", "This Account Number already exists.");
            }
            else
            {
                ModelState.AddModelError("", "An error occurred while saving the data.");
            }

            return View(ca);
        }
        catch (Exception)
        {
            ModelState.AddModelError("", "An unexpected error occurred.");
            return View(ca);
        }
    }


    [HttpGet]
    [Route("Edit/{id}")]
    public IActionResult Edit(int id)
    {
        var account = _caService.GetAccountById(id);
        if (account == null)
        {
            return NotFound();
        }
        return View("Edit", account);
    }
    [HttpPost]
    [Route("Edit/{id}")]
    public IActionResult Edit(int id, CA model)
    {
        if (!ModelState.IsValid)
        {
            return View("Edit", model);
        }

        _caService.UpdateAccount(model); 
        return RedirectToAction("ManageCA");
    }



    [HttpGet]
    [Route("ChartOfAccounts")]
    public IActionResult ChartOfAccounts()
    {
        var accounts = _caService.GetAllAccounts();
        return View("ChartOfAccounts", accounts);
    }



   
    [HttpPost]
    public IActionResult Delete(int id)
    {
        _caService.DeleteAccount(id);
        return RedirectToAction("ManageCA");
    }
}
