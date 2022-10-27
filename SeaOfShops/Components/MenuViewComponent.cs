using Microsoft.AspNetCore.Mvc;
using SeaOfShops.Entites;

namespace SeaOfShops.Components
{
    [ViewComponent]
    public class MenuViewComponent : ViewComponent
    {
        List<MenuItem> items = new List<MenuItem>
        {
            new MenuItem{Controller="Product", Action="Index", Text="List of all products", IsPage=false},
            new MenuItem{Controller="Order", Action="Index", Text="List of all orders", IsPage=false},
        };
        public IViewComponentResult Invoke()
        {
            if (Request.RouteValues["Controller"] is not null)
            {
                foreach (var item in items)
                {
                    item.Active = "disabled";
                }
                var controller = Request.RouteValues["Controller"].ToString();
                foreach (var item in items)
                {
                    if (item.Controller == controller)
                        item.Active = "active";
                }
            }
            if (Request.RouteValues["Area"] is not null)
            {
                var page = Request.RouteValues["Area"].ToString();
                foreach (var item in items)
                {
                    if (item.Page == ("/" + page))
                        item.Active = "active";
                }
            }
            return View(items);
        }
    }
}
