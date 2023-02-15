using Domain.Enums;
using Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StockTrackingAutomation.Web.Filters
{
	public class AuthFilterAttribute : ActionFilterAttribute
	{
		public AuthFilterAttribute()
		{

		}
		public AuthFilterAttribute(params RoleType[] roles)
		{
			rolesAllowed = roles;
		}
		private readonly RoleType[] rolesAllowed = Array.Empty<RoleType>();
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var authorized = context.HttpContext.IsAuthenticated();
			if(!authorized)
			{
				context.Result = new RedirectResult("/");
			}
			if(rolesAllowed.Length > 0)
			{
				var userRole = (RoleType)context.HttpContext.GetUser().RoleType;
				if(!rolesAllowed.Any(x => x == userRole))
				{
                    context.Result = new RedirectResult("/");
                }
			}
		}

	}
}
