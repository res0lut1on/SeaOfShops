using Microsoft.AspNetCore.Mvc;

namespace SeaOfShops.Components
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
