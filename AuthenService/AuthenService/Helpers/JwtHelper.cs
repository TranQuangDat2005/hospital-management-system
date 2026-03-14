using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using User_Authentication_Service.Model;

namespace User_Authentication_Service.Helpers
{
    public class JwtHelper
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiryMinutes;

        // Token blacklist: lưu các token đã logout (in-memory)
        private static readonly HashSet<string> _blacklistedTokens = new();
        private static readonly object _lock = new();

        public JwtHelper(IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            _secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey không được cấu hình");
            _issuer = jwtSettings["Issuer"] ?? "AuthenService";
            _audience = jwtSettings["Audience"] ?? "HospitalApp";
            _expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");
        }

        /// <summary>
        /// Tạo JWT token cho user đã xác thực.
        /// </summary>
        public (string Token, DateTime ExpiresAt) GenerateToken(Users user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiresAt = DateTime.UtcNow.AddMinutes(_expiryMinutes);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserID.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role ?? string.Empty),
                new Claim("FullName", user.FullName ?? string.Empty),
                new Claim("DepartmentID", user.DepartmentID.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
        }

        /// <summary>
        /// Thêm token vào blacklist khi logout.
        /// </summary>
        public bool BlacklistToken(string token)
        {
            lock (_lock)
            {
                return _blacklistedTokens.Add(token);
            }
        }

        /// <summary>
        /// Kiểm tra token có bị blacklist không.
        /// </summary>
        public bool IsBlacklisted(string token)
        {
            lock (_lock)
            {
                return _blacklistedTokens.Contains(token);
            }
        }

        /// <summary>
        /// Lấy principal từ token (kể cả expired).
        /// </summary>
        public ClaimsPrincipal? GetPrincipalFromToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = false // Cho phép đọc token hết hạn
            };

            try
            {
                var principal = new JwtSecurityTokenHandler()
                    .ValidateToken(token, tokenValidationParameters, out _);
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
