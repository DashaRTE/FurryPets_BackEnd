using FurryPets.Core.Interfaces;
using FurryPets.Core.Responses;
using System.Net;

namespace FurryPets.Core.UseCases;

public class EditCalendarNoteUseCase
{
    private readonly ICalendarNoteRepository _calendarNoteRepository;

    public EditCalendarNoteUseCase(ICalendarNoteRepository calendarNoteRepository)
    {
        _calendarNoteRepository = calendarNoteRepository;
    }

    public async Task<ResultResponse> HandleAsync(EditCalendarNoteRequest request)
    {
        var calendarNote = await _calendarNoteRepository.FindByIdAsync(request.CalendarNoteId);

        if (calendarNote is null)
        {
            return new() { StatusCode = HttpStatusCode.NotFound, Message = "CalendarNote not found" };
        }

        await _calendarNoteRepository.EditCalendarNoteAsync(request.CalendarNoteId, request.Reason, request.Note,
            request.Date, request.Time);

        await _calendarNoteRepository.CommitAsync();

        return new() { StatusCode = HttpStatusCode.OK };
    }
}