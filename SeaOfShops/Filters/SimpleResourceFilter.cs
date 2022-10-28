using Microsoft.AspNetCore.Mvc.Filters;

namespace SeaOfShops.Filters
{
    public class SimpleResourceFilter : Attribute, IResourceFilter
{
    int _id;
    string _token;
    public SimpleResourceFilter(int id, string token)
    {
        _id = id;
        _token = token;
    }
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        context.HttpContext.Response.Cookies.Append("LastVisit", DateTime.Now.ToString("dd/MM/yyyy hh-mm-ss"));
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        context.HttpContext.Response.Headers.Add("Id", _id.ToString());
        context.HttpContext.Response.Headers.Add("Token", _token);
    }
}
}
