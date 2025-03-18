namespace SmartAppointment.Domain.Entities
{
    public class Professional
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Specialization { get; set; } // This property exists
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string SLMC { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Professional(string name, string specialization, string email, string phoneNumber, string slmc)
        {
            Name = name;
            Specialization = specialization;
            Email = email;
            PhoneNumber = phoneNumber;
            SLMC = slmc;
        }

        public Professional(Guid id)
        {
            Id = id;
        }
        public Professional() { }

    }
}