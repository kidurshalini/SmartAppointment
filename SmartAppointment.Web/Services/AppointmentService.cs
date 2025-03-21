using SmartAppointment.Domain.Entities;

public class AppointmentService
{
    private readonly HttpClient _httpClient;

    public AppointmentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
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
    public async Task<bool> CreateAppointmentAsync(AppointmentModel appointment)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Appointments", appointment);
        return response.IsSuccessStatusCode;
    }


}
