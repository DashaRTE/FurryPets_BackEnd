using System.ComponentModel.DataAnnotations;

namespace FurryPets.API.Requests;

public record RefreshUserRequest([Required] string Token);
