using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRSystem.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected int TenantId
        {
            get
            {
                var claim = User.FindFirst("tenantId");

                if (claim == null)
                    throw new Exception("TenantId not found in token");

                return int.Parse(claim.Value);
            }
        }
    }
}