using Transit.Models;

namespace Transit;

public class Notifications
{
    public static Notifications Instance;

    public Action<int,int> StatementStatusChanged;

    public void Initialize()
    {
        if (Instance == null)
            Instance = this;
        else
            throw new Exception("Notifications initialized twice!");

        StatementStatusChanged += (statementId, status) => Console.WriteLine($"Statement with id: {statementId} changed, now status is: {(StatementStatus)status}");
    }
    
}