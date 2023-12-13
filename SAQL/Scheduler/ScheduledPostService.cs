using SAQL.Contexts;
using SAQL.Controllers;
using SAQL.Entities;

public class SchedulerService
{
    private Timer timer;
    private SAQLContext _context;

    public void SetContext(SAQLContext context)
    {
        _context = context;
    }
    public void Start()
    {
        //var dbContext = new SAQLContext();
        //var patient = dbContext.Patients.FirstOrDefault(d => d.Id == 23);
        //TimeSpan firstTime = CalculateTimeUntilNextExecution();
        //timer = new Timer(ExecuteTask, null, firstTime, patient.IntervalTime);
        var patient = GetPatientById(24); // Fetch patient details by ID

        if (patient == null)
        {
            // Handle patient not found
            return;
        }

        TimeSpan firstTime = CalculateTimeUntilNextExecution(patient);
        timer = new Timer(ExecuteTask, null, firstTime, patient.IntervalTime);
    }
    public void RestartTimer(long patientId, int interval, DateTime startTime)
    {
        var patient = GetPatientById(patientId);

        if (patient == null)
        {
            // Handle patient not found
            return;
        }

        // Stop the existing timer
        timer?.Change(Timeout.Infinite, 0);

        // Update patient details
        patient.IntervalTime = TimeSpan.FromHours(interval);
        patient.DateTime = startTime;

        // Calculate time until the next execution
        TimeSpan firstTime = CalculateTimeUntilNextExecution(patient);

        // Restart the timer with updated settings
        //timer?.Change(firstTime, patient.IntervalTime);

        timer = new Timer(ExecuteTask, null, firstTime, patient.IntervalTime);
    }
    private void ExecuteTask(object state)
    {
        //наш контролер
        var context = new SAQLContext();
        DataController controller = new DataController(context);
        controller.AddPhysical(24);
    }

    private TimeSpan CalculateTimeUntilNextExecution(Patient patient)
    {
        //var dbContext = new SAQLContext();
        //var patient = dbContext.Patients.FirstOrDefault(d => d.Id == 23);
        DateTime now = DateTime.Now;
        DateTime firstExecutionTime = new DateTime(now.Year, now.Month, now.Day, patient.DateTime.Hour, patient.DateTime.Minute, 0);

        if (now > firstExecutionTime)
        {
            firstExecutionTime = firstExecutionTime.AddDays(1);
        }

        return firstExecutionTime - now;
    }

    private Patient GetPatientById(long patientId)
    {
        var context = new SAQLContext();
        return context.Patients.FirstOrDefault(d => d.Id == patientId);
    }
}