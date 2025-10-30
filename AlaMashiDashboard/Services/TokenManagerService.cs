using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AlaMashiDashboard.Services;

public class TokenManagerService
{
    private readonly ProtectedLocalStorage _storage;
    private bool _isInitialized = false;

    // 📢 Events للإشعارات
    public event Action? OnUserNameChanged;
    public event Action? OnTokensCleared;

    public TokenManagerService(ProtectedLocalStorage storage)
    {
        _storage = storage;
    }

    private async Task EnsureInitializedAsync()
    {
        if (!_isInitialized)
        {
            await Task.Delay(100); // Small delay to ensure browser storage is ready
            _isInitialized = true;
        }
    }

    #region Token Management

    public async Task SetTokensAsync(string accessToken, string refreshToken)
    {
        await EnsureInitializedAsync();
        await _storage.SetAsync("accessToken", accessToken);
        await _storage.SetAsync("refreshToken", refreshToken);
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        try
        {
            await EnsureInitializedAsync();
            var result = await _storage.GetAsync<string>("accessToken");
            return result.Success ? result.Value : null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Error getting access token: {ex.Message}");
            return null;
        }
    }

    public async Task<string?> GetRefreshTokenAsync()
    {
        try
        {
            await EnsureInitializedAsync();
            var result = await _storage.GetAsync<string>("refreshToken");
            return result.Success ? result.Value : null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Error getting refresh token: {ex.Message}");
            return null;
        }
    }

    public async Task ClearTokensAsync()
    {
        try
        {
            await EnsureInitializedAsync();
            await _storage.DeleteAsync("accessToken");
            await _storage.DeleteAsync("refreshToken");
            await _storage.DeleteAsync("userName");
            await _storage.DeleteAsync("userRole");

            OnUserNameChanged?.Invoke();
            OnTokensCleared?.Invoke();

            Console.WriteLine("✅ Tokens cleared successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error clearing tokens: {ex.Message}");
        }
    }

    #endregion

    #region User Information

    public async Task SetUserNameAsync(string userName)
    {
        await EnsureInitializedAsync();
        await _storage.SetAsync("userName", userName);
        OnUserNameChanged?.Invoke();
    }

    public async Task<string?> GetUserNameAsync()
    {
        try
        {
            await EnsureInitializedAsync();
            var result = await _storage.GetAsync<string>("userName");

            if (result.Success && !string.IsNullOrEmpty(result.Value))
                return result.Value;

            // 🔄 Fallback: محاولة استخراج الاسم من Token
            return await GetUserNameFromTokenAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Error getting username: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// 🔍 استخراج اسم المستخدم من Token Claims
    /// </summary>
    private async Task<string?> GetUserNameFromTokenAsync()
    {
        try
        {
            var token = await GetAccessTokenAsync();
            if (string.IsNullOrEmpty(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var nameClaim = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == "name" ||
                c.Type == "Name" ||
                c.Type == ClaimTypes.Name ||
                c.Type == "unique_name" ||
                c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");

            return nameClaim?.Value;
        }
        catch
        {
            return null;
        }
    }

    public async Task<int?> GetUserIdAsync()
    {
        try
        {
            await EnsureInitializedAsync();
            var token = await GetAccessTokenAsync();
            if (string.IsNullOrEmpty(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // 🔍 البحث عن User ID في مختلف أنواع Claims
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == "sub" ||
                c.Type == "userId" ||
                c.Type == "user_id" ||
                c.Type == "uid" ||
                c.Type == "oid" ||
                c.Type == ClaimTypes.NameIdentifier ||
                c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
                return userId;

            Console.WriteLine($"⚠️ User ID claim not found in token");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error getting user ID from token: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// 🔐 استخراج UserRole من JWT Token
    /// يدعم جميع أنواع Role Claims المستخدمة في ASP.NET
    /// </summary>
    public async Task<string?> GetUserRoleAsync()
    {
        try
        {
            await EnsureInitializedAsync();
            var token = await GetAccessTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("⚠️ No access token found");
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // 🔍 البحث عن Role Claim بجميع الأسماء الممكنة
            var roleClaim = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == "role" ||
                c.Type == "Role" ||
                c.Type == ClaimTypes.Role ||
                c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" ||
                c.Type == "roles");

            if (roleClaim != null)
            {
                Console.WriteLine($"✅ User Role found: {roleClaim.Value}");
                return roleClaim.Value;
            }

            // 🔍 محاولة أخيرة: طباعة جميع الـ claims للتشخيص
            Console.WriteLine("⚠️ Role claim not found. Available claims:");
            foreach (var claim in jwtToken.Claims)
            {
                Console.WriteLine($"   - {claim.Type}: {claim.Value}");
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error getting user role from token: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// 🔐 التحقق من أن المستخدم هو Admin
    /// </summary>
    public async Task<bool> IsAdminAsync()
    {
        try
        {
            var role = await GetUserRoleAsync();
            var isAdmin = !string.IsNullOrEmpty(role) &&
                         role.Equals("Admin", StringComparison.OrdinalIgnoreCase);

            Console.WriteLine($"🔍 Admin check result: {isAdmin} (Role: {role})");
            return isAdmin;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error checking admin status: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 📧 استخراج البريد الإلكتروني من Token
    /// </summary>
    public async Task<string?> GetUserEmailAsync()
    {
        try
        {
            var token = await GetAccessTokenAsync();
            if (string.IsNullOrEmpty(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var emailClaim = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == "email" ||
                c.Type == ClaimTypes.Email ||
                c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

            return emailClaim?.Value;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Error getting email: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// 🔐 استخراج جميع معلومات المستخدم من Token
    /// </summary>
    public async Task<UserInfo?> GetUserInfoAsync()
    {
        try
        {
            var token = await GetAccessTokenAsync();
            if (string.IsNullOrEmpty(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userInfo = new UserInfo
            {
                UserId = await GetUserIdAsync(),
                UserName = await GetUserNameAsync(),
                Role = await GetUserRoleAsync(),
                Email = await GetUserEmailAsync(),
                ExpirationDate = jwtToken.ValidTo,
                IssuedAt = jwtToken.ValidFrom
            };

            Console.WriteLine($"📦 User Info Retrieved: ID={userInfo.UserId}, Role={userInfo.Role}, IsAdmin={userInfo.IsAdmin}");

            return userInfo;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error getting user info: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// 🔐 التحقق من صلاحية Token
    /// </summary>
    public async Task<bool> IsTokenValidAsync()
    {
        try
        {
            var token = await GetAccessTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("⚠️ No token to validate");
                return false;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var isValid = jwtToken.ValidTo > DateTime.UtcNow;

            if (!isValid)
            {
                Console.WriteLine($"⚠️ Token expired at {jwtToken.ValidTo:yyyy-MM-dd HH:mm:ss} UTC");
            }
            else
            {
                var timeLeft = jwtToken.ValidTo - DateTime.UtcNow;
                Console.WriteLine($"✅ Token valid. Expires in {timeLeft.TotalMinutes:F0} minutes");
            }

            return isValid;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error validating token: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 🔍 طباعة جميع Claims للتشخيص
    /// مفيد للـ debugging
    /// </summary>
    public async Task DebugPrintAllClaimsAsync()
    {
        try
        {
            var token = await GetAccessTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("⚠️ No token found for debugging");
                return;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            Console.WriteLine("========================================");
            Console.WriteLine("🔍 JWT TOKEN CLAIMS DEBUG INFO");
            Console.WriteLine("========================================");
            Console.WriteLine($"Issued: {jwtToken.ValidFrom:yyyy-MM-dd HH:mm:ss} UTC");
            Console.WriteLine($"Expires: {jwtToken.ValidTo:yyyy-MM-dd HH:mm:ss} UTC");
            Console.WriteLine($"Issuer: {jwtToken.Issuer}");
            Console.WriteLine("\nClaims:");

            foreach (var claim in jwtToken.Claims)
            {
                Console.WriteLine($"  • {claim.Type}: {claim.Value}");
            }

            Console.WriteLine("========================================");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error debugging claims: {ex.Message}");
        }
    }

    #endregion
}

/// <summary>
/// 📦 Model لتخزين معلومات المستخدم
/// </summary>
public class UserInfo
{
    public int? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Role { get; set; }
    public string? Email { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime IssuedAt { get; set; }

    /// <summary>
    /// 🔐 هل المستخدم Admin؟
    /// </summary>
    public bool IsAdmin => !string.IsNullOrEmpty(Role) &&
                          Role.Equals("Admin", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// ⏰ هل انتهت صلاحية Token؟
    /// </summary>
    public bool IsTokenExpired => ExpirationDate <= DateTime.UtcNow;

    /// <summary>
    /// ⏱️ الوقت المتبقي للـ Token
    /// </summary>
    public TimeSpan TimeRemaining => IsTokenExpired
        ? TimeSpan.Zero
        : ExpirationDate - DateTime.UtcNow;

    /// <summary>
    /// 📊 نسبة الوقت المتبقي للـ Token (0-100%)
    /// </summary>
    public double TokenLifetimePercentage
    {
        get
        {
            var totalLifetime = ExpirationDate - IssuedAt;
            if (totalLifetime.TotalSeconds <= 0) return 0;

            var remaining = TimeRemaining;
            if (remaining.TotalSeconds <= 0) return 0;

            return remaining.TotalSeconds / totalLifetime.TotalSeconds * 100;
        }
    }

    public override string ToString()
    {
        return $"User: {UserName} | ID: {UserId} | Role: {Role} | IsAdmin: {IsAdmin} | TokenValid: {!IsTokenExpired}";
    }
}