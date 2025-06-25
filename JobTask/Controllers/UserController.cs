using Microsoft.AspNetCore.Mvc;
using JobTask.Models;
using JobTask.Services;

namespace JobTask.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                _userService.GetAllUsers();
                ModelState.Clear();
            }

            ViewBag.Users = _userService.GetAllUsers();
            return View();
        }


        [HttpGet]
       
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpGet]
        [Route("AdminDashboard")]
        public IActionResult AdminDashboard()
        {
            return View("AdminDashboard");
        }

        [HttpGet]
        [Route("AccountantDashboard")]
        public IActionResult AccountantDashboard()
        {
            return View("AccountantDashboard");
        }

        [HttpGet]
        [Route("ViewerDashboard")]
        public IActionResult ViewerDashboard()
        {
            return View("ViewerDashboard");
        }

        



        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _userService.LoginUser(email, password);

            if (user != null)
            {
                HttpContext.Session.SetString("session_UserEmail", user.Email);
                HttpContext.Session.SetString("session_UserRole", user.Role);
                HttpContext.Session.SetString("session_UserFullName", user.FullName);



                if (string.Equals(user.Role, "Admin", StringComparison.OrdinalIgnoreCase))
                    return RedirectToAction("AdminDashboard");
                else if (string.Equals(user.Role, "Accountant", StringComparison.OrdinalIgnoreCase))
                    return RedirectToAction("AccountantDashboard");
                else if (string.Equals(user.Role, "Viewer", StringComparison.OrdinalIgnoreCase))
                    return RedirectToAction("ViewerDashboard");
                else
                {
                    ViewBag.ErrorMessage = "Invalid user role.";
                    return View("Login");
                }
            }

            ViewBag.ErrorMessage = "Invalid email or password.";
            return View("Login");
        }


    }
}

