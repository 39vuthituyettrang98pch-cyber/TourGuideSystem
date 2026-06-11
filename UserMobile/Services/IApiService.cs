using UserMobile.Models;

namespace UserMobile.Services;

public interface IApiService
{
    Task<ApiResponse<T>> GetAsync<T>(string route);
    Task<ApiResponse<T>> PostAsync<T>(string route, object body);
}
