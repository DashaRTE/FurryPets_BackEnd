using System.ComponentModel.DataAnnotations;

namespace FurryPets.API.Requests;

public record RegisterUserRequest([Required, StringLength(50)] string Username,
    [Required, EmailAddress] string Email, 
    [Required, DataType(DataType.Password)] string Password);