using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AppointmentAPI.DTOs;

namespace AppointmentAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentDbContext _context;

        public AppointmentsController(AppointmentDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return appointment;
        }

        [HttpPost]
        public async Task<ActionResult<Appointment>> CreateAppointment(AppointmentDto appointmentDto)
        {
            // Validate appointment date
            if (appointmentDto.AppointmentDateTime <= DateTime.Now)
            {
                return BadRequest("Appointment date must be in the future");
            }

            // Validate doctor exists
            var doctor = await _context.Doctors.FindAsync(appointmentDto.DoctorId);
            if (doctor == null)
            {
                return BadRequest("Invalid doctor ID");
            }

            var appointment = new Appointment
            {
                PatientName = appointmentDto.PatientName,
                PatientContact = appointmentDto.PatientContact,
                AppointmentDateTime = appointmentDto.AppointmentDateTime,
                DoctorId = appointmentDto.DoctorId
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, AppointmentDto appointmentDto)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            // Validate appointment date
            if (appointmentDto.AppointmentDateTime <= DateTime.Now)
            {
                return BadRequest("Appointment date must be in the future");
            }

            // Validate doctor exists
            var doctor = await _context.Doctors.FindAsync(appointmentDto.DoctorId);
            if (doctor == null)
            {
                return BadRequest("Invalid doctor ID");
            }

            appointment.PatientName = appointmentDto.PatientName;
            appointment.PatientContact = appointmentDto.PatientContact;
            appointment.AppointmentDateTime = appointmentDto.AppointmentDateTime;
            appointment.DoctorId = appointmentDto.DoctorId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}