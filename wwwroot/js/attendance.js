function filterAttendance() {
    var deptId = $('#filterDepartment').val();
    var empId = $('#filterEmployee').val();
    var from = $('#filterFrom').val();
    var to = $('#filterTo').val();

    $.post('/Attendance/Filter', { departmentId: deptId, employeeId: empId, from, to }, function (html) {
        $('#attendanceTable').html(html);
    });
}

$('#markEmployee, #markDate').on('change', function () {
    var empId = $('#markEmployee').val();
    var date = $('#markDate').val();
    if (!empId || !date) return;

    $.get('/Attendance/GetStatus', { employeeId: empId, date }, function (status) {
        $('#currentStatus').html('Current Status: <strong>' + status + '</strong>');
    });
});

function submitAttendance() {
    var empId = $('#markEmployee').val();
    var date = $('#markDate').val();
    var status = $('#markStatus').val();

    if (!empId || !date) {
        alert("Please select an employee and date.");
        return;
    }

    $.post('/Attendance/MarkAttendance', { employeeId: empId, date, status }, function (res) {
        if (res.success) {
            $('#currentStatus').html('Status Updated: <strong>' + res.status + '</strong>');
            filterAttendance();
        } else {
            alert(res.error);
        }
    });
}
