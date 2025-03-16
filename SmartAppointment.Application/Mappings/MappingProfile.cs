using AutoMapper;
using SmartAppointment.Domain.Entities;
using SmartAppointment.Web.Models;



namespace SmartAppointment.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // 🔹 Convert from DTO (AppointmentModel) to Entity (Appointment)
            CreateMap<AppointmentModel, Appointment>()
                .ForMember(dest => dest.Status, opt => opt.Ignore()); // Avoid modifying domain rules

            // 🔹 Convert from Entity (Appointment) to DTO (AppointmentModel) for API responses
            CreateMap<Appointment, AppointmentModel>().ReverseMap();
        }
    }
}