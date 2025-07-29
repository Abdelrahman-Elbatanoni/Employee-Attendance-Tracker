// === 1. Filter Attendance Table ===
function filterAttendance() {
    var deptId = $('#filterDepartment').val();
    var empId = $('#filterEmployee').val();
    var from = $('#filterFrom').val();
    var to = $('#filterTo').val();

    $.post('/Attendance/Filter', {
        departmentId: deptId,
        employeeId: empId,
        from: from,
        to: to
    }, function (html) {
        $('#attendanceTable').html(html);
    });
}

// === 2. Filter Employees (for filtering section) ===
$('#filterDepartment').on('change', function () {
    var departmentId = $(this).val();
    updateEmployeeDropdown('#filterEmployee', departmentId);
});

// === 3. Filter Employees (for marking attendance section) ===
$('#markDepartment').on('change', function () {
    var departmentId = $(this).val();
    updateEmployeeDropdown('#markEmployee', departmentId);
});

// === 4. Update Employee Dropdown by Department ===
function updateEmployeeDropdown(employeeSelectId, departmentId) {
    const $dropdown = $(employeeSelectId);
    $dropdown.empty().append('<option value="">-- Loading... --</option>');

    if (!departmentId) {
        $dropdown.html('<option value="">-- Select Employee --</option>');
        return;
    }

    $.get('/Attendance/GetEmployeesByDepartment', { departmentId }, function (employees) {
        $dropdown.empty().append('<option value="">-- Select Employee --</option>');
        $.each(employees, function (index, emp) {
            $dropdown.append(`<option value="${emp.id}">${emp.fullName}</option>`);
        });
    });
}

// === 5. Load Current Attendance Status ===
$('#markEmployee, #markDate').on('change', function () {
    var empId = $('#markEmployee').val();
    var date = $('#markDate').val();

    if (!empId || !date) return;

    $.get('/Attendance/GetStatus', { employeeId: empId, date: date }, function (status) {
        let message = (status === "NotMarked")
            ? "No attendance recorded for this date."
            : `Current Status: <strong>${status}</strong>`;
        $('#currentStatus').html(message);
    });
});

// === 6. Submit Attendance ===
function submitAttendance() {
    var empId = $('#markEmployee').val();
    var date = $('#markDate').val();
    var status = $('#markStatus').val();

    if (!empId || !date) {
        alert("Please select an employee and a date.");
        return;
    }

    $.post('/Attendance/MarkAttendance', {
        employeeId: empId,
        date: date,
        status: status
    }, function (res) {
        if (res.success) {
            $('#currentStatus').html(`Status Updated: <strong>${res.status}</strong>`);
            filterAttendance(); // Refresh table
        } else {
            alert(res.error);
        }
    });
}
