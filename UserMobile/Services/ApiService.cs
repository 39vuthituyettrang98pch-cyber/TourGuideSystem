using System.Net.Http.Headers;
using System.Net.Http.Json;
using UserMobile.Models;

namespace UserMobile.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<ApiResponse<T>> GetAsync<T>(string route)
    {
        var response = await _httpClient.GetAsync(route);
        if (!response.IsSuccessStatusCode)
        {
            return new ApiResponse<T> { Success = false, Message = response.ReasonPhrase ?? "Request failed." };
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
        return result ?? new ApiResponse<T> { Success = false, Message = "Could not parse response." };
    }

    public async Task<ApiResponse<T>> PostAsync<T>(string route, object body)
    {
        var response = await _httpClient.PostAsJsonAsync(route, body);
        if (!response.IsSuccessStatusCode)
        {
            return new ApiResponse<T> { Success = false, Message = response.ReasonPhrase ?? "Request failed." };
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
        return result ?? new ApiResponse<T> { Success = false, Message = "Could not parse response." };
    }
}
