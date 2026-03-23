using System;

namespace PropertyManagementConsole.App;

public static class MainMenu
{
    public static void Show()
    {
        while (true)
        {
            Console.WriteLine("\n=== Property Management System ===");
            Console.WriteLine("1) Manager");
            Console.WriteLine("2) Tenant");
            Console.WriteLine("0) Exit");
            Console.Write("Choose an option: ");

            string? userChoice = Console.ReadLine();

            if (userChoice == "0")
            {
                break;
            }

            if (userChoice == "1")
            {
                ManagerMenu.Show();
                continue;
            }

            if (userChoice == "2")
            {
                TenantMenu.LoginAndShow();
                continue;
            }

            Console.WriteLine("Invalid option. Try again.");
        }
    }
}
