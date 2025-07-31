
using AttendanceTracker.Data.Models;

namespace AttendanceTracker.Business.Services.Interfaces;

public interface IAttendanceService
{
    Task<IEnumerable<AttendanceRecord>> GetAllAsync();
    Task<PagedResult<AttendanceRecord>> GetPagedAttendanceRecords(int page, int pageSize);

    Task<AttendanceRecord> GetByIdAsync(int id);
    Task<AttendanceRecord> CreateOrUpdateAsync(int employeeId, DateTime date, AttendanceStatus status);
    Task<bool> DeleteAsync(int id);
    Task<AttendanceRecord> GetByEmployeeAndDateAsync(int employeeId, DateTime date);
    Task<List<AttendanceRecord>> FilterAsync(int? departmentId, int? employeeId, DateTime? from, DateTime? to);

}
