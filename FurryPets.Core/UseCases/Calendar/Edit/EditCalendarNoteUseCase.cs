using FurryPets.Core.Interfaces;
using FurryPets.Core.Responses;
using System.Net;

namespace FurryPets.Core.UseCases;

public class EditCalendarNoteUseCase
{
    private readonly ICalendarNoteRepository _calendarNoteRepository;
    private readonly IUserRepository _userRepository;

    public EditCalendarNoteUseCase(ICalendarNoteRepository calendarNoteRepository, IUserRepository userRepository)
    {
        _calendarNoteRepository = calendarNoteRepository;
        _userRepository = userRepository;
    }

    public async Task<ResultResponse> HandleAsync(EditCalendarNoteRequest request)
    {
        var calendarNote = await _calendarNoteRepository.FindByIdAsync(request.CalendarNoteId);

        if (calendarNote is null)
        {
            return new() { StatusCode = HttpStatusCode.NotFound, Message = "CalendarNote not found" };
        }

        if (calendarNote.UserId != request.UserId)
        {
            return new() { StatusCode = HttpStatusCode.BadRequest, Message = "CalendarNote belongs to another user" };
        }

        await _calendarNoteRepository.EditCalendarNoteAsync(request.CalendarNoteId, request.Reason, request.Note,
            request.Date, request.Time);

        await _userRepository.CommitAsync();

        return new() { StatusCode = HttpStatusCode.OK };
    }
}