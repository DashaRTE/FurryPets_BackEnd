namespace FurryPets.Core.Interfaces;
public interface IIsTokenIssuedService
{
    Task<bool> IsTokenIssuedAsync(string userId);
}
