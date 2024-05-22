using System.Net;
using FurryPets.Core.Interfaces;
using FurryPets.Core.Responses;

namespace FurryPets.Core.UseCases;
public class CreateCalendarNoteUseCase
{
    private readonly ICalendarNoteRepository _calendarNoteRepository;

    public CreateCalendarNoteUseCase(ICalendarNoteRepository calendarNoteRepository)
    {
        _calendarNoteRepository = calendarNoteRepository;
    }

    public async Task<ResultResponse> HandleAsync(CreateCalendarNoteRequest request)
    {
        await _calendarNoteRepository.CreateCalendarNoteAsync(request.UserId, request.Reason, request.Note, request.Date, request.Time);

        await _calendarNoteRepository.CommitAsync();

        return new() { StatusCode = HttpStatusCode.Created };
    }
}