using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System;
using System.Threading.Tasks;

public class TokenManagerService
{
    private readonly ProtectedLocalStorage _storage;

    public event Action? OnUserNameChanged;

    public TokenManagerService(ProtectedLocalStorage storage)
    {
        _storage = storage;
    }

    public async Task SetTokensAsync(string accessToken, string refreshToken)
    {
        await _storage.SetAsync("accessToken", accessToken);
        await _storage.SetAsync("refreshToken", refreshToken);
    }

    public async Task SetUserNameAsync(string userName)
    {
        await _storage.SetAsync("userName", userName);
        OnUserNameChanged?.Invoke();
    }

    public async Task<string?> GetUserNameAsync()
    {
        try
        {
            var result = await _storage.GetAsync<string>("userName");
            return result.Success ? result.Value : null;
        }
        catch { };

        return  null;
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        try
        {
            var result = await _storage.GetAsync<string>("accessToken");
            return result.Success ? result.Value : null;
        }
        catch { };
        return null;
    }

    public async Task<string?> GetRefreshTokenAsync()
    {
        try
        {
            var result = await _storage.GetAsync<string>("refreshToken");
            return result.Success ? result.Value : null;
        }
        catch { }
        return null;
    }

    public async Task ClearTokensAsync()
    {
        await _storage.DeleteAsync("accessToken");
        await _storage.DeleteAsync("refreshToken");
        await _storage.DeleteAsync("userName");
        OnUserNameChanged?.Invoke(); // 🔹 برضو بلّغ عند تسجيل الخروج
    }
}
