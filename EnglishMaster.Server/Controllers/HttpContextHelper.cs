using System.Security.Claims;

namespace EnglishMaster.Server.Controllers
{
    public static class HttpContextHelper
    {
        public static long GetUserId(this HttpContext context)
        {
            IEnumerable<Claim> claims = context.User.Claims;
            if (!claims.Any(a => a.Type == ClaimTypes.NameIdentifier))
            {
                throw new Exception("Invalid http context.");
            }
            string userIdValue = claims.First(a => a.Type == ClaimTypes.NameIdentifier).Value;
            return Convert.ToInt64(userIdValue);
        }
    }
}
