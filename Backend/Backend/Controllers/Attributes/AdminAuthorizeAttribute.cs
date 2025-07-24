//using Backend.Database;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using System.Security.Claims;

//public class AuthorizeUrlActionAttribute : Attribute, IAuthorizationFilter
//{
//    private readonly string _actionType;

//    public AuthorizeUrlActionAttribute(string actionType)
//    {
//        _actionType = actionType;
//    }

//    public void OnAuthorization(AuthorizationFilterContext context)
//    {
//        var user = context.HttpContext.User;

//        if (!user.Identity.IsAuthenticated)
//        {
//            context.Result = new ForbidResult();
//            return;
//        }

//        var isAdmin = user.IsInRole("Admin");
//        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

//        var routeId = context.RouteData.Values["id"]?.ToString();

//        if (_actionType == "Add")
//        {
//            return;
//        }
//        else if (_actionType == "Delete")
//        {
//            if (isAdmin)
//                return;

//            if (routeId != null)
//            {
//                var dbContext = context.HttpContext.RequestServices.GetService<AppDbContext>();
//                var url = dbContext.ShortUrls.Find(Guid.Parse(routeId));

//                if (url == null || url.CreatedById != Guid.Parse(userId))
//                {
//                    context.Result = new ForbidResult();
//                }
//            }
//        }
//    }
//}
