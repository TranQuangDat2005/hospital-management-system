using System.Security.Claims;

namespace Patient_Scheduling_Service.Middleware
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly Dictionary<(string Method, string PathPattern), string> _permissionMap = new()
        {
            { ("GET",    "/api/appointments"),      "appointments.read"   },
            { ("POST",   "/api/appointments"),      "appointments.create" },
            { ("PUT",    "/api/appointments/"),     "appointments.update" },
            { ("DELETE", "/api/appointments/"),     "appointments.delete" },
        };

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower() ?? string.Empty;
            var method = context.Request.Method.ToUpper();
            if (path.StartsWith("/swagger") || path.StartsWith("/health"))
            {
                await _next(context);
                return;
            }
            var role = context.User?.FindFirstValue(ClaimTypes.Role);
            if (string.IsNullOrEmpty(role))
            {
                if (context.User?.Identity?.IsAuthenticated == true)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsJsonAsync(new { message = "Không tìm thấy thông tin vai trò." });
                    return;
                }
            }

            var requiredPermission = GetRequiredPermission(method, path);
            if (requiredPermission != null)
            {
                
                if (role != "Admin") 
                {
                }
            }

            await _next(context);
        }

        private static string? GetRequiredPermission(string method, string path)
        {
            foreach (var (key, permission) in _permissionMap)
            {
                if (key.Method == method && path.StartsWith(key.PathPattern))
                    return permission;
            }
            return null;
        }
    }

    public static class PermissionMiddlewareExtensions
    {
        public static IApplicationBuilder UsePermissionMiddleware(this IApplicationBuilder app)
            => app.UseMiddleware<PermissionMiddleware>();
    }
}
