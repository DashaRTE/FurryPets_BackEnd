﻿using AutoMapper;
using FurryPets.Core.Dto;
using FurryPets.Core.Interfaces;
using FurryPets.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace FurryPets.Infrastructure.Repositories;

public class CalendarNoteRepository : ICalendarNoteRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public CalendarNoteRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IList<CalendarNoteDto?>> GetCalendarNotesAsync(string userId, DateOnly date)
    {
        var notes = await _context.CalendarNotes
            .Where(note => note.UserId == userId && note.Date == date)
            .ToListAsync();

        return _mapper.Map<IList<CalendarNote?>, IList<CalendarNoteDto?>>(notes);
    }



    public async Task<CalendarNoteDto?> FindByIdAsync(Guid calendarNoteId) =>
        _mapper.Map<CalendarNote?, CalendarNoteDto?>(await _context.CalendarNotes.FindAsync(calendarNoteId));

    public async Task<CalendarNoteDto> CreateCalendarNoteAsync(string userId, string? reason, string? note, DateOnly? date, TimeOnly? time)
    {
        var calendarNote = new CalendarNote { Id = Guid.NewGuid(), UserId = userId, Reason = reason, Note = note, Date = date, Time = time };

        await _context.CalendarNotes.AddAsync(calendarNote);

        return _mapper.Map<CalendarNote, CalendarNoteDto>(calendarNote);
    }

    public async Task<CalendarNoteDto?> EditCalendarNoteAsync(Guid calendarNoteId, string? reason, string? note, DateOnly? date, TimeOnly? time)
    {
        var calendarNote = await _context.CalendarNotes.FindAsync(calendarNoteId);

        if (calendarNote is not null)
        {
            calendarNote.Reason = reason;
            calendarNote.Note = note;
            calendarNote.Date = date;
            calendarNote.Time = time;
        }

        return _mapper.Map<CalendarNote?, CalendarNoteDto?>(calendarNote);
    }

    public async Task CommitAsync() => await _context.SaveChangesAsync();
}