﻿using System.ComponentModel.DataAnnotations;

namespace FurryPets.API.Requests;

public record CreateCalendarNoteRequest([StringLength(255)] string? Reason, [StringLength(1000)] string? Note,
    DateOnly? Date, TimeOnly? Time);