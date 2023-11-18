using FurryPets.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FurryPets.Infrastructure;
public class DataContext : IdentityDbContext<User>
{
    public DataContext(DbContextOptions options) : base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    public DataContext()
    {
        
    }

    public override DbSet<User> Users { get; set; }
    public virtual DbSet<Animal> Animals { get; set; }
    public virtual DbSet<CalendarNote> CalendarNotes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        Configuration.ModelBuilder.BuildUser(modelBuilder.Entity<User>());
        Configuration.ModelBuilder.BuildCalendarNote(modelBuilder.Entity<CalendarNote>());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter, DateOnlyComparer>()
            .HaveColumnType("date");

        configurationBuilder.Properties<TimeOnly>()
            .HaveConversion<TimeOnlyConverter, TimeOnlyComparer>();
    }

    private class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        public DateOnlyConverter() : base(static dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue), static dateTime => DateOnly.FromDateTime(dateTime)) { }
    }

    private class DateOnlyComparer : ValueComparer<DateOnly>
    {
        public DateOnlyComparer() : base(static (d1, d2) => d1.DayNumber == d2.DayNumber, static d => d.GetHashCode()) { }
    }

    private class TimeOnlyConverter : ValueConverter<TimeOnly, TimeSpan>
    {
        public TimeOnlyConverter() : base(static timeOnly => timeOnly.ToTimeSpan(), static timeSpan => TimeOnly.FromTimeSpan(timeSpan)) { }
    }

    private class TimeOnlyComparer : ValueComparer<TimeOnly>
    {
        public TimeOnlyComparer() : base(static (t1, t2) => t1.Ticks == t2.Ticks, static t => t.GetHashCode()) { }
    }
}
