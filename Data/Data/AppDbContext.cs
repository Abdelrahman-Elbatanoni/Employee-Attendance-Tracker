using AttendanceTracker.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceTracker.Data.Data;

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
            new Department { Id = 1, Name = "HR Management", Code = "HR01", Location = "Cairo" },
            new Department { Id = 2, Name = "IT Management", Code = "IT02", Location = "Alexandria" },
            new Department { Id = 3, Name = "Finance", Code = "FIN3", Location = "Mansoura" }
        );

        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, Code = 1001, FullName = "Ali Hassan Omar Mostafa", Email = "ali@company.com", DepartmentId = 1 },
            new Employee { Id = 2, Code = 1002, FullName = "Sara Ahmed Hossam Khaled", Email = "sara@company.com", DepartmentId = 2 },
            new Employee { Id = 3, Code = 1003, FullName = "Alice Mahmoud Tarek Khalil", Email = "alice@example.com", DepartmentId = 1 },
            new Employee { Id = 4, Code = 1004, FullName = "Bob Nasser Hossam Zaki", Email = "bob@example.com", DepartmentId = 1 },
            new Employee { Id = 5, Code = 1005, FullName = "Carol Ahmed Mohamed Elsayed", Email = "carol@example.com", DepartmentId = 2 },
            new Employee { Id = 6, Code = 1006, FullName = "David Youssef George Michele", Email = "david@example.com", DepartmentId = 2 },
            new Employee { Id = 7, Code = 1007, FullName = "Eve Adel Emad Hana", Email = "eve@example.com", DepartmentId = 3 },
            new Employee { Id = 8, Code = 1008, FullName = "Fady Khaled Mazen Aly", Email = "fady@example.com", DepartmentId = 3 },
            new Employee { Id = 9, Code = 1009, FullName = "Ghada Hani Ashraf Fathy", Email = "ghada@example.com", DepartmentId = 1 },
            new Employee { Id = 10, Code = 1010, FullName = "Hossam Ali Ibrahim Ahmed", Email = "hossam@example.com", DepartmentId = 2 },
            new Employee { Id = 11, Code = 1011, FullName = "Ibrahim Magdy Mostafa Tolba", Email = "ibrahim@example.com", DepartmentId = 3 }

        );

        modelBuilder.Entity<AttendanceRecord>().HasData(
            new AttendanceRecord { Id = 1, EmployeeId = 1, Date = new DateTime(2025, 7, 29), Status = AttendanceStatus.Present },
            new AttendanceRecord { Id = 2, EmployeeId = 2, Date = new DateTime(2025, 7, 29), Status = AttendanceStatus.Absent },
            new AttendanceRecord { Id = 3, EmployeeId = 3, Date = new DateTime(2025, 7, 28), Status = AttendanceStatus.Present },
            new AttendanceRecord { Id = 4, EmployeeId = 4, Date = new DateTime(2025, 7, 28), Status = AttendanceStatus.Present },
            new AttendanceRecord { Id = 5, EmployeeId = 5, Date = new DateTime(2025, 7, 29), Status = AttendanceStatus.Absent },
            new AttendanceRecord { Id = 6, EmployeeId = 6, Date = new DateTime(2025, 7, 29), Status = AttendanceStatus.Present },
            new AttendanceRecord { Id = 7, EmployeeId = 7, Date = new DateTime(2025, 7, 29), Status = AttendanceStatus.Absent },
            new AttendanceRecord { Id = 8, EmployeeId = 8, Date = new DateTime(2025, 7, 27), Status = AttendanceStatus.Present },
            new AttendanceRecord { Id = 9, EmployeeId = 9, Date = new DateTime(2025, 7, 27), Status = AttendanceStatus.Absent }

        );
    }
}
