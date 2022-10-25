using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SeaOfShops.Models;

namespace SeaOfShops.Components
{
    public class AvatarViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _hostEnvironment;


        public AvatarViewComponent(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this._hostEnvironment = hostEnvironment;
        }
        public async Task<IViewComponentResult> InvokeAsync(string user)
        {
            var _user = await _userManager.FindByNameAsync(user);
            return View(_user);
        }
    }
}
