using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FurryPets.Core.Interfaces;
using FurryPets.Core.Responses;

namespace FurryPets.Core.UseCases;

public class GetCalendarNotesUseCase
{
    private readonly ICalendarNoteRepository _calendarNoteRepository;

    public GetCalendarNotesUseCase(ICalendarNoteRepository calendarNoteRepository)
    {
        _calendarNoteRepository = calendarNoteRepository;
    }

    public async Task<ResultResponse<IList<GetCalendarNotesResponse>>> HandleAsync(GetCalendarNotesRequest request)
    {
        var notes = await _calendarNoteRepository.GetCalendarNotesAsync(request.UserId, request.Date);

        if (notes is null)
        {
            return new ResultResponse<IList<GetCalendarNotesResponse>>
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = "Calendar notes not found"
            };
        }

        var response = notes.Select(note => new GetCalendarNotesResponse(
            note.Id,
            note.Reason,
            note.Note,
            note.Date,
            note.Time
        )).ToList();

        return new ResultResponse<IList<GetCalendarNotesResponse>>
        {
            StatusCode = HttpStatusCode.OK,
            Data = response
        };
    }

}