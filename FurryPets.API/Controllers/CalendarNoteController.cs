using FurryPets.Core.UseCases;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FurryPets.API.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/calendarNote")]
public class CalendarNoteController : ControllerBase
{
    private readonly GetCalendarNotesUseCase _getCalendarNotesUseCase;
    private readonly CreateCalendarNoteUseCase _createCalendarNoteUseCase;
    private readonly EditCalendarNoteUseCase _editCalendarNoteUseCase;

    public CalendarNoteController(GetCalendarNotesUseCase getCalendarNotesUseCase,
        CreateCalendarNoteUseCase createCalendarNoteUseCase, EditCalendarNoteUseCase editCalendarNoteUseCase)
    {
        _getCalendarNotesUseCase = getCalendarNotesUseCase;
        _createCalendarNoteUseCase = createCalendarNoteUseCase;
        _editCalendarNoteUseCase = editCalendarNoteUseCase;
    }

    [HttpGet, Route("get")]
    [ProducesResponseType(typeof(List<GetCalendarNotesResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCalendarNotesAsync([Required, FromQuery] Requests.GetCalendarNoteRequest request)
    {
        var response = await _getCalendarNotesUseCase.HandleAsync(new(request.UserId, request.Date));

        if (string.IsNullOrWhiteSpace(response.Message))
        {
            return new ObjectResult(response.Data) { StatusCode = (int)response.StatusCode };
        }

        return new ObjectResult(response.Message) { StatusCode = (int)response.StatusCode };
    }


    [HttpPost, Route("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCalendarNotesAsync(
        [FromBody, Required] Requests.CreateCalendarNoteRequest request)
    {

        var response =
            await _createCalendarNoteUseCase.HandleAsync(new(request.UserId, request.Reason, request.Note, request.Date,
                request.Time));

        if (string.IsNullOrWhiteSpace(response.Message))
        {
            return new ObjectResult(response.Message) { StatusCode = (int)response.StatusCode };
        }

        return new ObjectResult(response.Message) { StatusCode = (int)response.StatusCode };
    }

    [HttpPut, Route("edit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> EditCalendarNotesAsync(
        [FromBody, Required] Requests.EditCalendarNoteRequest request)
    {

        var response = await _editCalendarNoteUseCase.HandleAsync(new(request.CalendarNoteId, request.Reason,
            request.Note, request.Date, request.Time));

        if (string.IsNullOrWhiteSpace(response.Message))
        {
            return new ObjectResult(response.Message) { StatusCode = (int)response.StatusCode };
        }

        return new ObjectResult(response.Message) { StatusCode = (int)response.StatusCode };
    }
}