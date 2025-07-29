using Employee_Attendance_Tracker.Models;
using Employee_Attendance_Tracker.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Employee_Attendance_Tracker.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;

        public EmployeeController(IEmployeeService employeeService, IDepartmentService departmentService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAllAsync();
            var summaries = new Dictionary<int, AttendanceSummary>();

            foreach (var emp in employees)
                summaries[emp.Id] = await _employeeService.GetAttendanceSummaryAsync(emp.Id);

            ViewBag.Summaries = summaries;
            return View(employees);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDepartments();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            var isunique = await _employeeService.IsEmailUniqueAsync(employee.Email);
            if (!isunique)
            {
                TempData["Error"] = "This email already exists.";
                return RedirectToAction("Create");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeService.CreateAsync(employee);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            await PopulateDepartments();
            return View(employee);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null) return NotFound();

            await PopulateDepartments();
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeService.UpdateAsync(employee);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            await PopulateDepartments();
            return View(employee);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null) return NotFound();

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _employeeService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDepartments()
        {
            var departments = await _departmentService.GetAllAsync();
            ViewBag.Departments = new SelectList(departments, "Id", "Name");
        }
    }
}
