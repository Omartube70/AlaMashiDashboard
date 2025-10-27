using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

public class ApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly TokenManagerService _tokenManager;
    private readonly NavigationManager _navManager;
    private readonly IConfiguration _config;

    public ApiService(
        IHttpClientFactory httpClientFactory,
        TokenManagerService tokenManager,
        NavigationManager navManager,
        IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _tokenManager = tokenManager;
        _navManager = navManager;
        _config = config;
    }

    // ------------------- Login -------------------
    public async Task<bool> LoginAsync(string email, string password)
    {
        var client = _httpClientFactory.CreateClient("Api");
        var loginCommand = new LoginCommand { Email = email, Password = password };

        try
        {
            var response = await client.PostAsJsonAsync("/api/Users/login", loginCommand);
            if (!response.IsSuccessStatusCode) 
                return false;

            var wrappedResponse = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>();
            var authData = wrappedResponse?.Data;

            if (authData != null && !string.IsNullOrEmpty(authData.AccessToken))
            {
                await _tokenManager.SetTokensAsync(authData.AccessToken, authData.RefreshToken);
                await _tokenManager.SetUserNameAsync(authData.User.UserName);
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login exception: {ex.Message}");
        }

        return false;
    }

    // ------------------- Refresh Token -------------------
    public async Task<bool> RefreshTokenAsync()
    {
        var refreshToken = await _tokenManager.GetRefreshTokenAsync();
        if (string.IsNullOrEmpty(refreshToken)) 
            return false;

        using var client = new HttpClient();
        var baseAddress = _config["ApiBaseAddress"];

        if (string.IsNullOrEmpty(baseAddress)) 
            return false;

        var requestBody = new { refreshToken };

        try
        {
            var response = await client.PostAsJsonAsync($"{baseAddress}/api/Users/refresh", requestBody);

            if (!response.IsSuccessStatusCode) 
                return false;

            var wrappedResponse = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>();
            var authData = wrappedResponse?.Data;

            if (authData != null && !string.IsNullOrEmpty(authData.AccessToken))
            {
                await _tokenManager.SetTokensAsync(authData.AccessToken, authData.RefreshToken);
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Refresh token exception: {ex.Message}");
        }

        return false;
    }

    // ------------------- GET API Call -------------------
    public async Task<T?> GetAsync<T>(string url)
    {
        var token = await _tokenManager.GetAccessTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            _navManager.NavigateTo("/login");
            return default;
        }

        var client = _httpClientFactory.CreateClient("Api");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        try
        {
            return await client.GetFromJsonAsync<T>(url);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            bool refreshed = await RefreshTokenAsync();

            if (refreshed) 
                return await GetAsync<T>(url);

            _navManager.NavigateTo("/login");
            return default;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GET exception: {ex.Message}");
            return default;
        }
    }

    // ------------------- POST API Call -------------------
    public async Task<T?> PostAsync<T>(string url, object body)
    {
        var token = await _tokenManager.GetAccessTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            _navManager.NavigateTo("/login");
            return default;
        }

        var client = _httpClientFactory.CreateClient("Api");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        try
        {
            var response = await client.PostAsJsonAsync(url, body);
            if (!response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                bool refreshed = await RefreshTokenAsync();
                if (refreshed) 
                    return await PostAsync<T>(url, body);

                _navManager.NavigateTo("/login");
                return default;
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"POST exception: {ex.Message}");
            return default;
        }
    }

    // دالة POST خاصة بالعمليات اللي مش محتاجة login
    public async Task<T?> PostWithoutAuthAsync<T>(string url, object body)
    {
        var client = _httpClientFactory.CreateClient("Api");

        try
        {
            var response = await client.PostAsJsonAsync(url, body);
            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (Exception ex)
        {
            return default;
        }
    }

    // دوال ForgotPassword / ResetPassword
    public async Task<bool> ForgotPasswordAsync(string email)
    {
        var request = new { email };
        var result = await PostWithoutAuthAsync<ApiResponse<object>>("/api/Users/forgot-password", request);
        return result != null;
    }

    public async Task<bool> ResetPasswordAsync(string email, string newPassword)
    {
        var request = new { email, password = newPassword };
        var result = await PostWithoutAuthAsync<ApiResponse<object>>("/api/Users/reset-password", request);
        return result != null;
    }


    // ------------------- Classes -------------------
    public class LoginCommand
    {
        [JsonPropertyName("email")] public string Email { get; set; } = string.Empty;
        [JsonPropertyName("password")] public string Password { get; set; } = string.Empty;
    }

    public class ApiResponse<T>
    {
        [JsonPropertyName("status")] public string Status { get; set; } = string.Empty;
        [JsonPropertyName("data")] public T Data { get; set; } = default!;
    }

    public class AuthResponse
    {
        [JsonPropertyName("accessToken")] public string AccessToken { get; set; } = string.Empty;
        [JsonPropertyName("refreshToken")] public string RefreshToken { get; set; } = string.Empty;
        [JsonPropertyName("user")] public UserDto User { get; set; } = new();
    }

    public class UserDto
    {
        [JsonPropertyName("userId")] public int UserId { get; set; }
        [JsonPropertyName("userName")] public string UserName { get; set; } = string.Empty;
    }
}
