using System.Net;
using FurryPets.Core.Interfaces;
using FurryPets.Core.Responses;

namespace FurryPets.Core.UseCases;
public class CreateCalendarNoteUseCase
{
    private readonly ICalendarNoteRepository _calendarNoteRepository;
    private readonly IUserRepository _userRepository;

    public CreateCalendarNoteUseCase(ICalendarNoteRepository calendarNoteRepository, IUserRepository userRepository)
    {
        _calendarNoteRepository = calendarNoteRepository;
        _userRepository = userRepository;
    }

    public async Task<ResultResponse> HandleAsync(CreateCalendarNoteRequest request)
    {
        await _calendarNoteRepository.CreateCalendarNoteAsync(request.UserId, request.Reason, request.Note, request.Date, request.Time);

        await _userRepository.CommitAsync();

        return new() { StatusCode = HttpStatusCode.Created };
    }
}