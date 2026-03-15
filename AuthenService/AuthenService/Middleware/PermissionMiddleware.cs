using System.Security.Claims;
using User_Authentication_Service.Interfaces;

namespace User_Authentication_Service.Middleware
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;

        private static readonly Dictionary<(string Method, string PathPattern), string> _permissionMap = new()
        {
            { ("GET",    "/api/users"),                         "users.read"          },
            { ("GET",    "/api/users/"),                        "users.read"          },
            { ("POST",   "/api/users"),                         "users.create"        },
            { ("PUT",    "/api/users/"),                        "users.update"        },
            { ("DELETE", "/api/users/"),                        "users.delete"        },
            { ("GET",    "/api/departments"),                   "departments.read"    },
            { ("GET",    "/api/departments/"),                  "departments.read"    },
            { ("POST",   "/api/departments"),                   "departments.create"  },
            { ("PUT",    "/api/departments/"),                  "departments.update"  },
            { ("DELETE", "/api/departments/"),                  "departments.delete"  },
            { ("GET",    "/api/permission"),                    "permissions.read"    },
            { ("POST",   "/api/permission"),                    "permissions.create"  },
            { ("DELETE", "/api/permission/"),                   "permissions.delete"  },
            { ("GET",    "/api/permission/roles"),              "permissions.read"    },
            { ("GET",    "/api/permission/roles/"),             "permissions.read"    },
            { ("POST",   "/api/permission/roles/"),             "permissions.assign"  },
            { ("DELETE", "/api/permission/roles/permissions/"), "permissions.assign"  },
        };

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAuthService authService, IPermissionRepository permissionRepo)
        {
            var path   = context.Request.Path.Value?.ToLower() ?? string.Empty;
            var method = context.Request.Method.ToUpper();

            if (IsPublicEndpoint(path))
            {
                await _next(context);
                return;
            }

            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            var token = authHeader?.StartsWith("Bearer ") == true
                ? authHeader["Bearer ".Length..]
                : null;

            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { message = "Token không được cung cấp." });
                return;
            }

            if (authService.IsTokenBlacklisted(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { message = "Token đã bị hủy, vui lòng đăng nhập lại." });
                return;
            }

            var role = context.User?.FindFirstValue(ClaimTypes.Role);
            if (string.IsNullOrEmpty(role))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new { message = "Không xác định được vai trò." });
                return;
            }

            var requiredPermission = GetRequiredPermission(method, path);
            if (requiredPermission != null)
            {
                var hasPermission = await permissionRepo.HasPermissionAsync(role, requiredPermission);
                if (!hasPermission)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        message = $"Bạn không có quyền thực hiện thao tác này. Cần quyền: {requiredPermission}"
                    });
                    return;
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

        private static bool IsPublicEndpoint(string path)
        {
            return path.StartsWith("/api/auth/login")
                || path.StartsWith("/api/auth/register")
                || path.StartsWith("/swagger")
                || path.StartsWith("/health");
        }
    }

    public static class PermissionMiddlewareExtensions
    {
        public static IApplicationBuilder UsePermissionMiddleware(this IApplicationBuilder app)
            => app.UseMiddleware<PermissionMiddleware>();
    }
}
