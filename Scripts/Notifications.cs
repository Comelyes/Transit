using Transit.Extensions;
using Transit.Models;

namespace Transit;

/// <summary>
/// Система уведомлений, отвечает за отправку данных об изменений заданий (Id и Статус)
/// </summary>
public class Notifications
{
    public static Notifications Instance;

    /// <summary>
    /// Событие изменения статуса задания, вызывается при обработке изменений задания в контроллере 
    /// </summary>
    public Action<int,int> StatementStatusChanged;

    public void Initialize()
    {
        if (Instance == null)
            Instance = this;
        else
            throw new Exception("Notifications initialized twice!");

        StatementStatusChanged += HandleStatementStatusChanged;
    }

    private void HandleStatementStatusChanged(int statementId, int status)
    {
        Console.WriteLine($"Statement with id: {statementId} changed, now status is: {(StatementStatus)status}");
        try
        {
            ApiRequests.PostRequest("https://someservertohandle.com/api/StatementStatusChanged", $"statementId={statementId}&status={status}");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error while sending notification!");
        }
    }
    
}