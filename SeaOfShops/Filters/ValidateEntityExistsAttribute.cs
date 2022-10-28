using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SeaOfShops.Data;
using SeaOfShops.Models;

namespace SeaOfShops.Filters
{
    public class ValidateEntityExistsAttribute<T> : IActionFilter where T : class, IEntity
    {
        private readonly ApplicationContext _context;
        public ValidateEntityExistsAttribute(ApplicationContext context)
        {
            _context = context;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var id = 0;

            if (context.ActionArguments.ContainsKey("id"))
            {
                id = (int)context.ActionArguments["id"];
            }
            else
            {
                context.Result = new BadRequestObjectResult("Bad id parameter");
                return;
            }

            var entity = _context.Set<T>().SingleOrDefault(x => x.Id.Equals(id));
            if(entity == null)
            {
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("entity", entity);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        { }

    }
}
