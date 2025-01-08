namespace AppointmentAPI.DTOs
{
    public class AppointmentDto
    {
        public string PatientName { get; set; } = string.Empty;
        public string PatientContact { get; set; } = string.Empty;
        public DateTime AppointmentDateTime { get; set; }
        public int DoctorId { get; set; }
    }
}