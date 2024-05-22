using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;

namespace FurryPets.API.Requests;

public record EditCalendarNoteRequest([Required] string UserId, [Required] Guid CalendarNoteId,[StringLength(255)] string? Reason, [StringLength(1000)] string? Note,
    DateOnly? Date, TimeOnly? Time);
