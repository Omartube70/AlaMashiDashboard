using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Threading.Tasks;

public class TokenManagerService
{
    private readonly ProtectedLocalStorage _storage;
    private bool _isInitialized = false;

    public event Action? OnUserNameChanged;

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

    public async Task SetTokensAsync(string accessToken, string refreshToken)
    {
        await EnsureInitializedAsync();
        await _storage.SetAsync("accessToken", accessToken);
        await _storage.SetAsync("refreshToken", refreshToken);
    }

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
            return result.Success ? result.Value : null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting username: {ex.Message}");
            return null;
        }
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
            Console.WriteLine($"Error getting access token: {ex.Message}");
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
            Console.WriteLine($"Error getting refresh token: {ex.Message}");
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

            // حاول العثور على user ID من مختلف أسماء الـ claims
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == "sub" ||
                c.Type == "userId" ||
                c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier" ||
                c.Type == "oid");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
                return userId;

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting user ID from token: {ex.Message}");
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
            OnUserNameChanged?.Invoke();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error clearing tokens: {ex.Message}");
        }
    }
}

