using AttendanceTracker.Data.Models;

namespace AttendanceTracker.Business.Services.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<PagedResult<Employee>> GetPagedEmployees(int page, int pageSize);
    Task<PagedResult<Employee>> GetPagedEmployees(int page, int pageSize, string sortBy, string sortOrder);

    Task<Employee> GetByIdAsync(int id);

    Task<Employee> CreateAsync(Employee employee);
    Task<Employee> UpdateAsync(Employee employee);
    Task<bool> DeleteAsync(int id);
    Task<bool> IsEmailUniqueAsync(int id, string email);
    Task<AttendanceSummary> GetAttendanceSummaryAsync(int employeeId);
}
