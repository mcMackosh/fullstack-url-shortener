using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Backend.Controllers.Attributes
{
    public class AdminOnlyAttribute : AuthorizeAttribute
    {
        public AdminOnlyAttribute()
        {
            Roles = "ADMIN";
        }
    }
}
