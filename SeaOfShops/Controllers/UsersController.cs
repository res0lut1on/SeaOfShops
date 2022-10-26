using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SeaOfShops.LoggerProvider;
using SeaOfShops.Models;
using SeaOfShops.ViewModels;
using System.Data;

namespace SeaOfShops.Controllers
{
   // [Authorize(Roles = AdminRole)]
    public class UsersController : Controller
    {
        private readonly ILogger _logger;
        private UserManager<User> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private const string AdminRole = "admin";
       
        public UsersController(UserManager<User> userManager, IWebHostEnvironment hostEnvironment, ILoggerFactory logger)
        {
            _logger = logger.CreateLogger("MyUsers");
            _userManager = userManager;
            this._hostEnvironment = hostEnvironment;
        }
        public void OnGet()
        {
            _logger.LogInformation("About page visited at {DT}",
                        DateTime.UtcNow.ToLongTimeString());
        }

        public IActionResult Index() => View(_userManager.Users.ToList());
        
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            var routeInfo = Request.Path + model.Email;
            _logger.Log(LogLevel.Information, MyLogEvents.CreateItem, routeInfo);
            _logger.LogInformation(MyLogEvents.CreateItem, routeInfo); 

            if (ModelState.IsValid)
            {
                string? ImageName = null;
                if (model.ImageFile is not null)
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
                    return RedirectToAction("Index");
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

        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, RealName = user.RealName, LastName = user.LastName, ImageName = user.ImageName };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            var ss = model.ImageFile;
            var s1 = model.ImageName;
             
            if (ModelState.IsValid)
            {
                if(model.ImageName == "none_avatar")
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                    string extension = Path.GetExtension(model.ImageFile.FileName);
                    model.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }
                }
                if (model.ImageFile != null)
                {
                    // delete old avatar
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    var fileName = "";
                    fileName = model.ImageName;

                    var fullPath = wwwRootPath + "/Images/" + model.ImageName;

                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                        ViewBag.deleteSuccess = "true";
                    }

                    // add new avatar
                    fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                    string extension = Path.GetExtension(model.ImageFile.FileName);
                    model.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }
                }

                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.RealName = model.RealName;
                    user.LastName = model.LastName;
                    user.ImageName = model.ImageName;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
                return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    IdentityResult result =
                        await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(model);
        }
    }
}
