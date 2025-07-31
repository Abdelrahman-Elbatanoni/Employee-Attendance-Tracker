using AttendanceTracker.Data.Models;

namespace AttendanceTracker.Business.Services.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<Department>> GetAllAsync();
    Task<PagedResult<Department>> GetPagedDepartments(int page, int pageSize);

    Task<Department> GetByIdAsync(int id);
    Task<Department> CreateAsync(Department department);
    Task<Department> UpdateAsync(Department department);
    Task<bool> DeleteAsync(int id);
    Task<int> GetEmployeeCountAsync(int departmentId);
    Task<bool> IsCodeUniqueAsync(int id,string code);
    Task<bool> IsNameUniqueAsync(int id,string name);
}
