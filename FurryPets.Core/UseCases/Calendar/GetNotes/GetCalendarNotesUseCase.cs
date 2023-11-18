using FurryPets.Core.Interfaces;
using FurryPets.Core.Responses;
using System.Net;

namespace FurryPets.Core.UseCases;

public class GetCalendarNotesUseCase
{
    private readonly ICalendarNoteRepository _calendarNoteRepository;
    private readonly IUserRepository _userRepository;

    public GetCalendarNotesUseCase(ICalendarNoteRepository calendarNoteRepository, IUserRepository userRepository)
    {
        _calendarNoteRepository = calendarNoteRepository;
        _userRepository = userRepository;
    }

    public async Task<ResultResponse<IList<GetCalendarNotesResponse>>> HandleAsync(GetCalendarNotesRequest request)
    {
        var user = await _userRepository.FindByIdAsync(request.UserId);

        if (user is null)
        {
            return new() { StatusCode = HttpStatusCode.NotFound, Message = "User not found" };
        }

        var notes = await _calendarNoteRepository.GetCalendarNotesAsync(request.UserId);

        return new()
        {
            StatusCode = HttpStatusCode.OK,
            Data = notes.Select(note =>
                new GetCalendarNotesResponse(note.Id, note.Reason, note.Note, note.Date, note.Time)).ToList()
        };
    }
}