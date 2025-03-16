using SmartAppointment.Web.Models;

namespace SmartAppointment.Web.Services
{
    public class AppointmentService
    {
        private readonly HttpClient _httpClient;

        public AppointmentService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BaseUrl");
        }

        public async Task<List<AppointmentModel>> GetAppointmentsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<AppointmentModel>>("api/Appointments");
        }

        public async Task<List<AppointmentModel>> GetUserAppointmentsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<AppointmentModel>>("api/Appointments/user");
        }

        public async Task<List<AppointmentModel>> GetProfessionalAppointmentsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<AppointmentModel>>("api/Appointments/professional");
        }

        public async Task<AppointmentModel?> CreateAppointmentAsync(AppointmentModel appointment)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Appointments", appointment);
            return response.IsSuccessStatusCode ?
                   await response.Content.ReadFromJsonAsync<AppointmentModel>() : null;
        }

        public async Task<bool> UpdateAppointmentAsync(AppointmentModel appointment)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Appointments/{appointment.Id}", appointment);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAppointmentAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Appointments/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
