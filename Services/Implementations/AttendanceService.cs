using Employee_Attendance_Tracker.Data;
using Employee_Attendance_Tracker.Models;
using Employee_Attendance_Tracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Employee_Attendance_Tracker.Services.Implementations;
public class AttendanceService : IAttendanceService
{
    private readonly AppDbContext _context;

    public AttendanceService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AttendanceRecord>> GetAllAsync()
    {
        return await _context.AttendanceRecords
            .Include(a => a.Employee).ThenInclude(e => e.Department)
            .OrderByDescending(a => a.Date)
            .ToListAsync();
    }

    public async Task<AttendanceRecord> GetByIdAsync(int id)
    {
        return await _context.AttendanceRecords
            .Include(a => a.Employee)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<AttendanceRecord> CreateOrUpdateAsync(int employeeId, DateTime date, AttendanceStatus status)
    {
        if (date.Date > DateTime.Today)
            throw new Exception("Cannot mark attendance for future dates.");

        var existing = await _context.AttendanceRecords
            .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date.Date == date.Date);

        if (existing != null)
        {
            existing.Status = status;
        }
        else
        {
            existing = new AttendanceRecord
            {
                EmployeeId = employeeId,
                Date = date.Date,
                Status = status
            };
            _context.AttendanceRecords.Add(existing);
        }

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<AttendanceRecord> GetByEmployeeAndDateAsync(int employeeId, DateTime date)
    {
        return await _context.AttendanceRecords
            .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date.Date == date.Date);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var record = await _context.AttendanceRecords.FindAsync(id);
        if (record == null) return false;

        _context.AttendanceRecords.Remove(record);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<AttendanceRecord>> FilterAsync(int? departmentId, int? employeeId, DateTime? from, DateTime? to)
    {
        var query = _context.AttendanceRecords
            .Include(a => a.Employee)
            .ThenInclude(e => e.Department)
            .AsQueryable();

        if (departmentId.HasValue)
            query = query.Where(a => a.Employee.DepartmentId == departmentId);

        if (employeeId.HasValue)
            query = query.Where(a => a.EmployeeId == employeeId);

        if (from.HasValue)
            query = query.Where(a => a.Date >= from.Value);

        if (to.HasValue)
            query = query.Where(a => a.Date <= to.Value);

        return await query.OrderByDescending(a => a.Date).ToListAsync();
    }
}
