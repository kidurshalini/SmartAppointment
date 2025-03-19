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

        public async Task<bool> AddScheduleAsync(scheduleModel scheduleModel)
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
    
}
