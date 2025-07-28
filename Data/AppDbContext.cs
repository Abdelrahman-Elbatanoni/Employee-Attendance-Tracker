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
        base.OnModelCreating(modelBuilder);
       
        
        modelBuilder.Entity<Department>()
            .HasIndex(d => d.Name).IsUnique();
        modelBuilder.Entity<Department>()
            .HasIndex(d => d.Code).IsUnique();

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Email).IsUnique();

        modelBuilder.Entity<AttendanceRecord>()
            .HasIndex(a => new { a.EmployeeId, a.Date }).IsUnique();


        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "HR Management", Code = "HRMG", Location = "Cairo" },
            new Department { Id = 2, Name = "Tech Team", Code = "TECH", Location = "Alexandria" }
        );

        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, Code = 1001, FullName = "Ali Hassan Omar Mostafa", Email = "ali@company.com", DepartmentId = 1 },
            new Employee { Id = 2, Code = 1002, FullName = "Sara Ahmed Hossam Khaled", Email = "sara@company.com", DepartmentId = 2 }
        );

        modelBuilder.Entity<AttendanceRecord>().HasData(
            new AttendanceRecord { Id = 1, EmployeeId = 1, Date = DateTime.Today.AddDays(-1), Status = AttendanceStatus.Present },
            new AttendanceRecord { Id = 2, EmployeeId = 2, Date = DateTime.Today.AddDays(-1), Status = AttendanceStatus.Absent }
        );
    }
}
