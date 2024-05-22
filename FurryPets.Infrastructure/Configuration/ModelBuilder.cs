using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FurryPets.Infrastructure.Entities;

namespace FurryPets.Infrastructure.Configuration;
public static class ModelBuilder
{
    public static void BuildCalendarNote(EntityTypeBuilder<CalendarNote> builder)
    {
        builder.Property(static calendar => calendar.Reason)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(static calendar => calendar.Note)
            .HasMaxLength(1000)
            .IsRequired();
    }
}
