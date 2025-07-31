# 🕒 Employee Attendance Tracker

An ASP.NET Core MVC web application to manage departments, employees, and track employee attendance with support for filtering, validation, pagination, and CRUD operations.

---

## 🚀 Features

### ✅ Departments
- Create, edit, delete, and view departments
- Each department has:
  - Name
  - Code
  - Location
- Paginated view (2 departments per page)

---

### ✅ Employees
- Create, edit, delete, and view employees
- Each employee belongs to one department
- Email must be unique (duplicate check with popup alert)
- Read-only fields supported (e.g., generated codes)
- Paginated view (5 employees per page)

---

### ✅ Attendance Management
- Track daily attendance for each employee
- Status options: `Present`, `Absent`
- Filter by department, employee, and date
- **Dynamic employee dropdown** based on selected department
- Mark attendance directly from the UI
- View, edit, and delete attendance records
- Paginated view (3 records per page)

---

## 🧪 Validation
- Required fields are validated on both client and server
- Errors are shown inline using Bootstrap alert styling
- Duplicate department's codes and names errors shown in popup alerts
- Duplicate email errors shown in popup alerts

---

## 🛠️ Tech Stack
-N-tier Architecture (presentation, business, and data)
- ASP.NET Core MVC
- Entity Framework Core (In-Memory Database)
- Bootstrap 5
- jQuery (for dynamic dropdowns and AJAX filters)

---

### Attendance Records
- Several attendance records seeded for testing
- Includes multiple days and employees

---


---

## 🧰 Setup Instructions

1. Clone the repository
2. Open the project in Visual Studio
3. Run the project — the in-memory database is auto-seeded on start
4. Navigate to `/Department`, `/Employee`, or `/Attendance`

---

## 📝 Notes

- All data is stored in-memory and will reset on each run
- For persistent data, switch to SQL Server and update `DbContext` configuration
- Responsive design is based on Bootstrap 5
