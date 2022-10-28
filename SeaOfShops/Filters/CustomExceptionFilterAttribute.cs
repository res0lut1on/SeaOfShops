using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace SeaOfShops.Filters
{
    public class CustomExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            string actionName = context.ActionDescriptor.DisplayName;
            string exceptionStack = context.Exception.StackTrace;
            string exceptionMessage = context.Exception.Message;
            
            context.Result = new ContentResult
            {
                Content = $"In method {actionName} exception: \n {exceptionMessage} \n {exceptionStack} \n DateTime -> {DateTime.Now.ToString()}"
            };
            context.ExceptionHandled = true;
        }
    }
}
