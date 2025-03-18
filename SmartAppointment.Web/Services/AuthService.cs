//using SmartAppointment.Web.Models;

//namespace SmartAppointment.Web.Services
//{
//    public class AuthService
//    {
//        private readonly HttpClient _http;
//        private readonly IHttpClientFactory _httpClientFactory;
//        public AuthService(HttpClient http, IHttpClientFactory httpClientFactory)
//        {
//            _httpClientFactory = httpClientFactory;
//            _http = _httpClientFactory.CreateClient("BaseUrl");
//        }

//        public async Task<string> LoginAsync(LoginModel model)
//        {
//            var response = await _http.PostAsJsonAsync("api/Auth/login", model);
//            if (response.IsSuccessStatusCode)
//            {
//                // Assuming the API returns a JSON object with a "token" property
//                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
//                return result?.Token;
//            }
//            return null;
//        }

//        public async Task<bool> RegisterAsync(RegisterModel model)
//        {
//            var response = await _http.PostAsJsonAsync("api/Auth/register", model);
//            return response.IsSuccessStatusCode;
//        }
//    }

//    public class LoginResponse
//    {
//        public string Token { get; set; }
//    }
//}
using Microsoft.JSInterop;
using SmartAppointment.Web.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace SmartAppointment.Web.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _jsRuntime;

        // Single constructor with all required dependencies
        public AuthService(HttpClient http, IHttpClientFactory httpClientFactory, IJSRuntime jsRuntime)
        {
            _http = httpClientFactory.CreateClient("BaseUrl"); // Use the named HttpClient
            _jsRuntime = jsRuntime;
        }

        //public async Task<string> LoginAsync(LoginModel model)
        //{
        //    var response = await _http.PostAsJsonAsync("api/Auth/login", model);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        // Assuming the API returns a JSON object with a "token" property
        //        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        //        return result?.Token;
        //    }
        //    return null;
        //}
        public async Task<string> LoginAsync(LoginModel model)
        {
            var response = await _http.PostAsJsonAsync("api/Auth/login", model);
            if (response.IsSuccessStatusCode)
            {
                // Assuming the API returns a JSON object with a "token" property
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (!string.IsNullOrEmpty(result?.Token))
                {
                    // Store the token in localStorage
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", result.Token);
                    return result.Token;
                }
            }
            return null;
        }
        public async Task Logout()
        {
            // Remove the token from localStorage
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");

            // Optionally, you can also clear other user-related data
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userDetails");

        }

        public async Task<bool> RegisterAsync(RegisterModel model)
        {
            var response = await _http.PostAsJsonAsync("api/Auth/register", model);
            return response.IsSuccessStatusCode;
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }
}