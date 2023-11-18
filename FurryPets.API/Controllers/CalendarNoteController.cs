using FurryPets.Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace FurryPets.API.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/calendarNote")]
[Authorize]
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
    [ProducesResponseType(typeof(IList<GetCalendarNotesResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCalendarNotesAsync()
    {
        var userId = HttpContext.User.Claims.First(static claim => claim.Type == ClaimTypes.NameIdentifier).Value;

        var response = await _getCalendarNotesUseCase.HandleAsync(new(userId));

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
        var userId = HttpContext.User.Claims.First(static claim => claim.Type == ClaimTypes.NameIdentifier).Value;

        var response =
            await _createCalendarNoteUseCase.HandleAsync(new(userId, request.Reason, request.Note, request.Date,
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
        var userId = HttpContext.User.Claims.First(static claim => claim.Type == ClaimTypes.NameIdentifier).Value;

        var response = await _editCalendarNoteUseCase.HandleAsync(new(request.CalendarNoteId, userId, request.Reason,
            request.Note, request.Date, request.Time));

        if (string.IsNullOrWhiteSpace(response.Message))
        {
            return new ObjectResult(response.Message) { StatusCode = (int)response.StatusCode };
        }

        return new ObjectResult(response.Message) { StatusCode = (int)response.StatusCode };
    }
}