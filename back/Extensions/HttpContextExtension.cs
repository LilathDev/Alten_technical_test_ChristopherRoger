using System.Security.Claims;

namespace back.Extensions
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Retrieves the user ID from the current HTTP context.
        /// </summary>
        public static Guid? GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
                return null;

            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.Authentication);

            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }

            return null;
        }
    }
}