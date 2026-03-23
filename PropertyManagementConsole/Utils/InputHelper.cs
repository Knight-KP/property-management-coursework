using System;

namespace PropertyManagementConsole.Utils;

public static class InputHelper
{
    public static int? ReadInt(string prompt)
    {
        Console.Write(prompt);

        return int.TryParse(Console.ReadLine(), out int number) ? number : null;
    }

    public static decimal? ReadDecimal(string prompt)
    {
        Console.Write(prompt);

        return decimal.TryParse(Console.ReadLine(), out decimal amount) ? amount : null;
    }

    public static DateTime? ReadDate(string prompt)
    {
        Console.Write(prompt);

        return DateTime.TryParse(Console.ReadLine(), out DateTime enteredDate) ? enteredDate : null;
    }

    public static string ReadRequiredText(string prompt)
    {
        Console.Write(prompt);

        return (Console.ReadLine() ?? string.Empty).Trim();
    }

    public static bool AskYesNo(string prompt)
    {
        Console.Write(prompt);

        string answer = (Console.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();
        return answer == "y" || answer == "yes";
    }
}
