using Employee_Attendance_Tracker.Models;
using Employee_Attendance_Tracker.Services.Implementations;
using Employee_Attendance_Tracker.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Attendance_Tracker.Controllers;

public class DepartmentController : Controller
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        const int pageSize = 2;

        var pagedDeparmtents = await _departmentService.GetPagedDepartments(page, pageSize);
        ViewBag.CurrentPage = page;

        return View(pagedDeparmtents);
    }


    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Department department)
    {
        var isCodeunique = await _departmentService.IsCodeUniqueAsync(department.Code);
        var isNameunique = await _departmentService.IsNameUniqueAsync(department.Name);

        if (!isCodeunique)
        {
            TempData["Error"] = "This code already exists.";
            return RedirectToAction("Create");
        }
        if (!isNameunique)
        {
            TempData["Error"] = "This name already exists.";
            return RedirectToAction("Create");
        }


        if (ModelState.IsValid)
        {
            try
            {
                await _departmentService.CreateAsync(department);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
        }
        return View(department);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var department = await _departmentService.GetByIdAsync(id);
        if (department == null) return NotFound();
        return View(department);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Department department)
    {
        if (id != department.Id) return BadRequest();

        var isCodeunique = await _departmentService.IsCodeUniqueAsync(department.Code);
        var isNameunique = await _departmentService.IsNameUniqueAsync(department.Name);

        if (!isCodeunique)
        {
            TempData["Error"] = "This code already exists.";
            return RedirectToAction("Edit", new { Id = id });
        }
        if (!isNameunique)
        {
            TempData["Error"] = "This name already exists.";
            return RedirectToAction("Edit", new { Id = id });
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _departmentService.UpdateAsync(department);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
        }
        return View(department);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var department = await _departmentService.GetByIdAsync(id);
        if (department == null) return NotFound();
        return View(department);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _departmentService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
