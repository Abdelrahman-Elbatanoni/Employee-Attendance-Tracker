using Employee_Attendance_Tracker.Models;
using Microsoft.EntityFrameworkCore;

namespace Employee_Attendance_Tracker.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<AttendanceRecord> AttendanceRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>()
            .HasIndex(d => d.Name).IsUnique();
        modelBuilder.Entity<Department>()
            .HasIndex(d => d.Code).IsUnique();

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Email).IsUnique();

        modelBuilder.Entity<AttendanceRecord>()
            .HasIndex(a => new { a.EmployeeId, a.Date }).IsUnique();
    }
}
