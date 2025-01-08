namespace AppointmentAPI.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string PatientContact { get; set; } = string.Empty;
        public DateTime AppointmentDateTime { get; set; }
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
    }
}