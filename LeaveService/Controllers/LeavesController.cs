using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LeaveService.Data; 
using LeaveService.Models; 
using LeaveService.Services;

namespace LeaveService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeavesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILeaveProcessingService _processingService;

        public LeavesController(AppDbContext context, ILeaveProcessingService processingService)
        {
            _context = context;
            _processingService = processingService;
        }

        // --- ACTIONS DE L'EMPLOYÉ ---

        // Appelé par Angular à chaque changement de date 
        [HttpPost("simulate-impact")]
        public async Task<IActionResult> SimulateImpact([FromBody] LeaveRequest request)
        {
            // Simulation de la logique de couverture d'équipe
            // une requête DB ici pour compter les absents de la CellId
            double mockCoverage = 75.0; 
            
            return Ok(new 
            { 
                percentage = mockCoverage, 
                message = "Impact équipe : Modéré",
                status = "Warning" 
            });
        }

        // Soumettre la demande finale
        [HttpPost("submit")]
        public async Task<IActionResult> Create([FromBody] LeaveRequest request)
        {
            if (request == null) return BadRequest();

            // 1. Contrôle de cohérence (Date Fin > Date Début)
            if (request.EndDate <= request.StartDate)
            {
                return BadRequest("La date de fin doit être après la date de début.");
            }

            // 2. Initialisation
            request.Status = LeaveStatus.Pending;
            request.CreatedAt = DateTime.UtcNow;

            // 3. Sauvegarde PostgreSQL
            _context.LeaveRequests.Add(request);
            await _context.SaveChangesAsync();

            // 4.  publier un message "Absence_Created" ici via RabbitMQ
            
            return Ok(new { id = request.Id, message = "Demande enregistrée avec succès !", leave = request });
        }

        [HttpGet("my-requests/{employeeId}")]
        public async Task<ActionResult<IEnumerable<LeaveRequest>>> GetMyRequests(string employeeId)
        {
            return await _context.LeaveRequests
                .Where(r => r.EmployeeId == employeeId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(Guid id) // Changé en Guid pour correspondre à ton modèle
        {
            var leave = await _context.LeaveRequests.FindAsync(id);
            if (leave == null) return NotFound();

            _context.LeaveRequests.Remove(leave);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // --- ACTIONS DU MANAGER ---

        [HttpPost("{id}/validate")]
        public async Task<IActionResult> Validate(Guid id) // Changé en Guid
        {
            var leave = await _context.LeaveRequests.FindAsync(id);
            if (leave == null) return NotFound();

            leave.Status = LeaveStatus.Approved;
            await _context.SaveChangesAsync();

            // 5. Déclenchement du workflow RabbitMQ pour le PlanningEngine et Notification
            await _processingService.ValidateLeaveAsync(
                leave.Id, 
                leave.EmployeeId, 
                leave.StartDate, 
                leave.EndDate
            );

            return Ok(new { Message = "Demande validée et événements RabbitMQ publiés", Leave = leave });
        }
    }
}