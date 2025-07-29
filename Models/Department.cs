using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Employee_Attendance_Tracker.Models;

public class Department
{
    public int Id { get; set; }

    [Required, StringLength(50, MinimumLength = 3)]
    public string Name { get; set; }

    [Required, RegularExpression(@"^[A-Z]{4}$")]
    public string Code { get; set; }

    [Required, StringLength(100)]
    public string Location { get; set; }

    [ValidateNever]
    public ICollection<Employee> Employees { get; set; }
}
