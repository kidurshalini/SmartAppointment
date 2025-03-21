 using SmartAppointment.Domain.Entities;
using SmartAppointment.Web.Models;

public class ScheduleService
    {
        private readonly HttpClient _httpClient;

        // Constructor to inject the HttpClient
        public ScheduleService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BaseUrl");
        }

        public async Task<bool> AddScheduleAsync(SmartAppointment.Domain.Entities.scheduleModel scheduleModel)
        {
            try
            {
                // Send the request to the API
                var response = await _httpClient.PostAsJsonAsync("api/Schedule", scheduleModel);

                // Check if the response is successful
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("schedule added successfully.");
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
    public async Task<List<SmartAppointment.Web.Models.ScheduleModel>> GetScheduleAsync(string appointmentDateFilter = "")
    {
        try
        {
            var apiUrl = "api/schedule";
            if (!string.IsNullOrEmpty(appointmentDateFilter))
            {
                apiUrl += $"?appointmentDateFilter={Uri.EscapeDataString(appointmentDateFilter)}";
            }

            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<SmartAppointment.Web.Models.ScheduleModel>>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching schedules: {ex.Message}");
            return new List<SmartAppointment.Web.Models.ScheduleModel>();
        }
    }
    public async Task<SmartAppointment.Web.Models.ScheduleModel> GetScheduleByIdAsync(Guid scheduleId)
    {
        // Fetch the schedule from the API
        var response = await _httpClient.GetAsync($"/api/Schedule/{scheduleId}");

        if (response.IsSuccessStatusCode)
        {
            // Deserialize the response content into ScheduleModel
            return await response.Content.ReadFromJsonAsync<SmartAppointment.Web.Models.ScheduleModel>();
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null; // Schedule not found
        }
        else
        {
            // Throw an exception with the status code if the request fails
            throw new Exception($"Failed to fetch schedule: {response.StatusCode}");
        }
    }
    public async Task<bool> UpdateScheduleAsync(SmartAppointment.Web.Models.ScheduleModel scheduleModel)
    {
        // Update the schedule via the API
        var response = await _httpClient.PutAsJsonAsync($"/api/Schedule/{scheduleModel.Id}", scheduleModel);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteScheduleAsync(Guid scheduleId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/Schedule/{scheduleId}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting schedule: {ex.Message}");
            return false;
        }
    }
}
