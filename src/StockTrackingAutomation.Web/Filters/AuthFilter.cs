using Domain.Helpers;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StockTrackingAutomation.Web.Filters
{
	public class AuthFilterAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var authorized = context.HttpContext.IsAuthenticated();
			if(!authorized)
			{
				context.Result = new RedirectResult("/");
			}
		}

	}
}
