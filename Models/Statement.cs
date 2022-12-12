namespace Transit.Models;

public class Statement
{
    public int Id { get; set; }
    public int SeekerId { get; set; }
    public int Status { get; set; }
    public int Value { get; set; }
    public string PassTime { get; set; } = "0";
    public int SuperVisorId { get; set; }

    public static string GetStringStatusRus(StatementStatus status)
    {
        switch (status)
        {
            case StatementStatus.Created:
                return "Создан";
            case StatementStatus.Done:
                return "Закончен";
            case StatementStatus.Checked:
                return "Проверен";
            case StatementStatus.Expired:
                return "Просрочен";
            default:
                return "Error in status!";
        }
    }
}

