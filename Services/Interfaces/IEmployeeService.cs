using Employee_Attendance_Tracker.Models;

namespace Employee_Attendance_Tracker.Services.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee> GetByIdAsync(int id);

    Task<Employee> CreateAsync(Employee employee);
    Task<Employee> UpdateAsync(Employee employee);
    Task<bool> DeleteAsync(int id);
    Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null);
    Task<AttendanceSummary> GetAttendanceSummaryAsync(int employeeId);
}
