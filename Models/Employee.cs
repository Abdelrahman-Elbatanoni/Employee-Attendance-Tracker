using System.ComponentModel.DataAnnotations;

namespace Employee_Tracker.Models;

public class Employee
{
    public int Id { get; set; }

    [Required]
    public int Code { get; set; } // Will be generated in service

    [Required, RegularExpression(@"^([A-Za-z]{2,}\s){3}[A-Za-z]{2,}$")]
    public string FullName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public int DepartmentId { get; set; }
    public Department Department { get; set; }

    public ICollection<AttendanceRecord> AttendanceRecords { get; set; }
}
