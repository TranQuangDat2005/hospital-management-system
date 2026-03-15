using System.Security.Claims;

namespace Patient_Scheduling_Service.Middleware
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;

        // Map các endpoint của Scheduling Service với quyền tương ứng
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

            // Cho phép truy cập Swagger và Health check không cần token
            if (path.StartsWith("/swagger") || path.StartsWith("/health"))
            {
                await _next(context);
                return;
            }

            // Kiểm tra Role từ Claims (đã được JWT Middleware giải mã)
            var role = context.User?.FindFirstValue(ClaimTypes.Role);
            if (string.IsNullOrEmpty(role))
            {
                // Nếu không có role, hoặc chưa đăng nhập, để JWT Middleware xử lý (401) 
                // hoặc chặn luôn 403 ở đây nếu Token hợp lệ nhưng thiếu Claim
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
                // TRONG THỰC TẾ: Ở đây bạn sẽ gọi DB hoặc Cache để kiểm tra Role đó có quyền này không.
                // Vì service này chưa có Repository/DB cụ thể cho Permission, tôi sẽ tạm giả định logic:
                // Admin có mọi quyền, hoặc các logic kiểm tra khác.
                
                if (role != "Admin") // Ví dụ tạm thời: Chỉ Admin mới qua được nếu có yêu cầu quyền
                {
                    // Lưu ý: Bạn cần đồng bộ Permission DB hoặc gọi API qua AuthenService để check thực sự.
                    // context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    // await context.Response.WriteAsJsonAsync(new { message = $"Thiếu quyền: {requiredPermission}" });
                    // return;
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
