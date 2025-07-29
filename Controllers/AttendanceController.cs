using Employee_Attendance_Tracker.Models;
using Employee_Attendance_Tracker.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Employee_Attendance_Tracker.Controllers;

public class AttendanceController : Controller
{
    private readonly IAttendanceService _attendanceService;
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;

    public AttendanceController(
        IAttendanceService attendanceService,
        IEmployeeService employeeService,
        IDepartmentService departmentService)
    {
        _attendanceService = attendanceService;
        _employeeService = employeeService;
        _departmentService = departmentService;
    }

    public async Task<IActionResult> Index()
    {
        await PopulateFilters();
        var records = await _attendanceService.GetAllAsync();
        return View(records);
    }

    [HttpPost]
    public async Task<IActionResult> Filter(int? departmentId, int? employeeId, DateTime? from, DateTime? to)
    {
        var filtered = await _attendanceService.FilterAsync(departmentId, employeeId, from, to);
        return PartialView("AttendanceTable", filtered);
    }

    [HttpGet]
    public async Task<IActionResult> GetStatus(int employeeId, DateTime date)
    {
        var record = await _attendanceService.GetByEmployeeAndDateAsync(employeeId, date.Date);
        if (record == null) return Json("NotMarked");
        return Json(record.Status.ToString());
    }

    [HttpPost]
    public async Task<IActionResult> MarkAttendance(int employeeId, DateTime date, AttendanceStatus status)
    {
        try
        {
            var record = await _attendanceService.CreateOrUpdateAsync(employeeId, date, status);
            return Json(new { success = true, status = record.Status.ToString() });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployeesByDepartment(int departmentId)
    {
        var employees = await _employeeService.GetAllAsync();
        var filtered = employees
            .Where(e => e.DepartmentId == departmentId)
            .Select(e => new { e.Id, e.FullName })
            .ToList();

        return Json(filtered);
    }


    private async Task PopulateFilters()
    {
        ViewBag.Departments = new SelectList(await _departmentService.GetAllAsync(), "Id", "Name");
        ViewBag.Employees = new SelectList(await _employeeService.GetAllAsync(), "Id", "FullName");
    }
}
