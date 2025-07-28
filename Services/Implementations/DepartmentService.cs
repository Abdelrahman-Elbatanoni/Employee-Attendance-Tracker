using Employee_Attendance_Tracker.Data;
using Employee_Attendance_Tracker.Services.Interfaces;
using Employee_Tracker.Models;
using Microsoft.EntityFrameworkCore;

namespace Employee_Attendance_Tracker.Services.Implementations;

public class DepartmentService : IDepartmentService
{
    private readonly AppDbContext _context;

    public DepartmentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await _context.Departments.Include(d => d.Employees).ToListAsync();
    }

    public async Task<Department> GetByIdAsync(int id)
    {
        return await _context.Departments.FindAsync(id);
    }

    public async Task<Department> CreateAsync(Department department)
    {
        if (await _context.Departments.AnyAsync(d => d.Code == department.Code || d.Name == department.Name))
            throw new Exception("Department code or name already exists.");

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
        return department;
    }

    public async Task<Department> UpdateAsync(Department department)
    {
        var existing = await _context.Departments.FindAsync(department.Id);
        if (existing == null) return null;

        if ((existing.Name != department.Name && await _context.Departments.AnyAsync(d => d.Name == department.Name)) ||
            (existing.Code != department.Code && await _context.Departments.AnyAsync(d => d.Code == department.Code)))
            throw new Exception("Duplicate department name or code.");

        existing.Name = department.Name;
        existing.Code = department.Code;
        existing.Location = department.Location;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department == null) return false;

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> GetEmployeeCountAsync(int departmentId)
    {
        return await _context.Employees.CountAsync(e => e.DepartmentId == departmentId);
    }

    public async Task<bool> IsCodeUniqueAsync(string code)
    {
        return !await _context.Departments.AnyAsync(d => d.Code == code);
    }

    public async Task<bool> IsNameUniqueAsync(string name)
    {
        return !await _context.Departments.AnyAsync(d => d.Name == name);
    }
}
