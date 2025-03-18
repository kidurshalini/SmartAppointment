//using System;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Threading.Tasks;
//using SmartAppointment.Web.Models;

//public class ProfessionalService
//{
//    private readonly HttpClient _httpClient;

//    // Constructor to inject the HttpClient
//    public ProfessionalService(IHttpClientFactory httpClientFactory)
//    {
//        _httpClient = httpClientFactory.CreateClient("BaseUrl");
//    }

//    public async Task<bool> AddProfessionalAsync(ProfessionalModel professional)
//    {
//        try
//        {
//            // Send the request to the API
//            var response = await _httpClient.PostAsJsonAsync("api/professional", professional);

//            // Check if the response is successful
//            if (response.IsSuccessStatusCode)
//            {
//                Console.WriteLine("Professional added successfully.");
//                return true;
//            }
//            else
//            {
//                var errorMessage = await response.Content.ReadAsStringAsync();
//                Console.WriteLine($"Error: {response.StatusCode}, {errorMessage}");
//                return false;
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Exception occurred: {ex.Message}");
//            return false;
//        }
//    }

//    public async Task<List<ProfessionalModel>> GetProfessionalsAsync(string specialization = "")
//    {
//        try
//        {
//            string apiUrl = "api/professional";
//            if (!string.IsNullOrWhiteSpace(specialization))
//            {
//                apiUrl += $"?specialization={Uri.EscapeDataString(specialization)}";
//            }

//            return await _httpClient.GetFromJsonAsync<List<ProfessionalModel>>(apiUrl) ?? new List<ProfessionalModel>();
//        }
//        catch (HttpRequestException httpEx)
//        {
//            Console.WriteLine($"HTTP Request failed: {httpEx.Message}");
//            return new List<ProfessionalModel>();
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Error fetching professionals: {ex.Message}");
//            return new List<ProfessionalModel>();
//        }
//    }
//}
using SmartAppointment.Domain.Entities;
using SmartAppointment.Web.Models;

public class ProfessionalService
{
    private readonly HttpClient _httpClient;

    // Constructor to inject the HttpClient
    public ProfessionalService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("BaseUrl");
    }

    public async Task<bool> AddProfessionalAsync(ProfessionalModel professional)
    {
        try
        {
            // Send the request to the API
            var response = await _httpClient.PostAsJsonAsync("api/professional", professional);

            // Check if the response is successful
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Professional added successfully.");
                return true;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {response.StatusCode}, {errorMessage}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
            return false;
        }
    }

    public async Task<List<ProfessionalModel>> GetProfessionalsAsync(string specialization = "")
    {
        try
        {
            string apiUrl = "api/professional";
            if (!string.IsNullOrWhiteSpace(specialization))
            {
                apiUrl += $"?specialization={Uri.EscapeDataString(specialization)}";
            }

            return await _httpClient.GetFromJsonAsync<List<ProfessionalModel>>(apiUrl) ?? new List<ProfessionalModel>();
        }
        catch (HttpRequestException httpEx)
        {
            Console.WriteLine($"HTTP Request failed: {httpEx.Message}");
            return new List<ProfessionalModel>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching professionals: {ex.Message}");
            return new List<ProfessionalModel>();
        }
    }

    public async Task<ProfessionalModel> GetProfessionalByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"api/professional/{id}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ProfessionalModel>();
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null; // Professional not found
        }
        else
        {
            throw new Exception($"Failed to fetch professional: {response.StatusCode}");
        }
    }

    public async Task<bool> UpdateProfessionalAsync(ProfessionalModel professional)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/professional/{professional.Id}", professional);
        return response.IsSuccessStatusCode;
    }



    public async Task<bool> DeleteProfessionalAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/professional/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
