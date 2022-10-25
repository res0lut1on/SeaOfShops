using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using SeaOfShops.Models;
using SeaOfShops.ViewModels;

namespace SeaOfShops.Controllers
{
    public class AccountController : Controller
    {
        #region  Fields
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        #endregion Fields
        #region Method
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this._hostEnvironment = hostEnvironment;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string? ImageName = null;
                if (model.ImageFile != null)
                {
                    //save to wwwroot
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                    string extension = Path.GetExtension(model.ImageFile.FileName);
                    ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }
                }

                if (ImageName is null)
                    ImageName = "niko.jpg";

                User user = new User { Email = model.Email, UserName = model.Email, RealName = model.RealName, LastName = model.LastName, ImageName = ImageName };               
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {                    
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
    }
    #endregion Method
}
