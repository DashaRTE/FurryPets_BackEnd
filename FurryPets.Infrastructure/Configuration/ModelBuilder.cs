using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FurryPets.Infrastructure.Entities;

namespace FurryPets.Infrastructure.Configuration;
public static class ModelBuilder
{
    public static void BuildUser(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(user => user.Animals)
            .WithOne(animal => animal.User)
            .HasForeignKey(animal => animal.UserId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasMany(user => user.CalendarNotes)
            .WithOne(calendar => calendar.User)
            .HasForeignKey(calendar => calendar.UserId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.Property(static user => user.CreationDate)
            .HasDefaultValueSql("getutcdate()");
    }

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
