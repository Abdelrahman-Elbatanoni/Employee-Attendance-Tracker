namespace AttendanceTracker.Data.Models;

public class AttendanceSummary
{
    public int Presents { get; set; }
    public int Absents { get; set; }
    public double AttendancePercentage { get; set; }
}
