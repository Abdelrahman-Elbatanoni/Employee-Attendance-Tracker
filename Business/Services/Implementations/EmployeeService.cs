using AttendanceTracker.Data.Data;
using AttendanceTracker.Business.Services.Interfaces;
using AttendanceTracker.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceTracker.Business.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly AppDbContext _context;

    public EmployeeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _context.Employees.Include(e => e.Department).ToListAsync();
    }

    public async Task<PagedResult<Employee>> GetPagedEmployees(int page, int pageSize)
    {
        var total = await _context.Employees.CountAsync();
        var employees= await _context.Employees
            .Include(e=>e.Department)
            .OrderBy(e => e.FullName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return new PagedResult<Employee>() { Items = employees , Page=page, PageSize=pageSize,TotalCount=total};
    }


    public async Task<Employee> GetByIdAsync(int id)
    {
        return await _context.Employees.Include(e => e.Department).FirstOrDefaultAsync(e => e.Id == id);
    }
    public async Task<Employee> CreateAsync(Employee employee)
    {

        // Validate name: 4 words, each ≥ 2 letters
        var names = employee.FullName?.Trim().Split(' ');
        if (names == null || names.Length != 4 || names.Any(n => n.Length < 2))
            throw new Exception("Full name must contain exactly 4 names, each at least 2 characters.");

        // Generate unique employee code
        employee.Code = await GenerateUniqueCodeAsync();
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task<Employee> UpdateAsync(Employee employee)
    {
        var existing = await _context.Employees.FindAsync(employee.Id);
        if (existing == null) return null;

        var names = employee.FullName?.Trim().Split(' ');
        if (names == null || names.Length != 4 || names.Any(n => n.Length < 2))
            throw new Exception("Full name must contain exactly 4 names, each at least 2 characters.");

        existing.FullName = employee.FullName;
        existing.Email = employee.Email;
        existing.DepartmentId = employee.DepartmentId;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return false;

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsEmailUniqueAsync(int id,string email)
    {
        return !await _context.Employees.AnyAsync(e => e.Email == email && e.Id != id);
    }

    private async Task<int> GenerateUniqueCodeAsync()
    {
        int maxCode = await _context.Employees.MaxAsync(e => (int?)e.Code) ?? 1000;
        return maxCode + 1;
    }

    public async Task<AttendanceSummary> GetAttendanceSummaryAsync(int employeeId)
    {
        var now = DateTime.Now;
        var records = await _context.AttendanceRecords
            .Where(a => a.EmployeeId == employeeId && a.Date.Month == now.Month && a.Date.Year == now.Year)
            .ToListAsync();

        int present = records.Count(r => r.Status == AttendanceStatus.Present);
        int absent = records.Count(r => r.Status == AttendanceStatus.Absent);
        int total = present + absent;

        return new AttendanceSummary
        {
            Presents = present,
            Absents = absent,
            AttendancePercentage = total == 0 ? 0 : Math.Round((double)present / total * 100, 2)
        };
    }
}
