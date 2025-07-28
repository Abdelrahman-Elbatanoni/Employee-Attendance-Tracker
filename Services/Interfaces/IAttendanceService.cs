
using Employee_Attendance_Tracker.Models;

namespace Employee_Attendance_Tracker.Services.Interfaces;

public interface IAttendanceService
{
    Task<IEnumerable<AttendanceRecord>> GetAllAsync();
    Task<AttendanceRecord> GetByIdAsync(int id);
    Task<AttendanceRecord> CreateOrUpdateAsync(int employeeId, DateTime date, AttendanceStatus status);
    Task<bool> DeleteAsync(int id);
    Task<AttendanceRecord> GetByEmployeeAndDateAsync(int employeeId, DateTime date);
    Task<IEnumerable<AttendanceRecord>> FilterAsync(int? departmentId, int? employeeId, DateTime? from, DateTime? to);

}
