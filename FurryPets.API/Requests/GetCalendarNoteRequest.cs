using System.ComponentModel.DataAnnotations;

namespace FurryPets.API.Requests;

public record GetCalendarNoteRequest([Required] string UserId, [Required] DateOnly Date);
