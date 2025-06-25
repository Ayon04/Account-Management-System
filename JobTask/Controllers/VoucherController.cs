using JobTask.Models;
using JobTask.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobTask.Controllers
{
    public class VoucherController : Controller
    {
        private readonly VoucherService _voucherService;

        public VoucherController(VoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            ViewBag.Vouchers = _voucherService.GetAllVouchers();
            return View("Create"); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public IActionResult Create(VoucherEntries entry)
        {
            if (ModelState.IsValid)
            {
                _voucherService.InsertVoucher(entry);

                
                ViewBag.Vouchers = _voucherService.GetAllVouchers();
                ModelState.Clear(); 
                return View(); 
            }

           
            ViewBag.Vouchers = _voucherService.GetAllVouchers();
            return View(entry);
        }
    }
}
