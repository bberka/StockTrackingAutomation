using EasMe.Exceptions;
using EasMe.Logging;
using EasMe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StockTrackingAutomation.Web.Filters
{
    public class ExceptionHandleFilter : IExceptionFilter
    {
        private static readonly IEasLog logger = EasLogFactory.CreateLogger();

        public void OnException(ExceptionContext context)
        {
            var query = context.HttpContext.Request.QueryString;
            //context.HttpContext.Response.StatusCode = 500;

            var type = context.Exception.GetType();
            if (type.Equals(typeof(NotAuthorizedException)))
            {
                //context.HttpContext.Response.StatusCode = 403;
                context.Result = new RedirectToActionResult("Statistics", "Home",null);
                //Logging 
            }
            else
            {
                context.Result = new ObjectResult(Result.Error(100, context.Exception.Message));
                //context.HttpContext.Response.StatusCode = 500;
                logger.Exception(context.Exception, $"Query({query})");
                context.Result = new RedirectToActionResult("Statistics", "Home", null);

            }

        }

    }
}
