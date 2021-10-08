using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace KeyVaultUtility
{
    class Program
    {
        static Settings azSettings;
        static void Main(string[] args)
        {
            IConfiguration cfg = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            azSettings = cfg.GetSection(nameof(Settings)).Get<Settings>();

            MainMenu();
        }

        static void MainMenu()
        {
            var kvService = new KeyVault(azSettings);
            ConsoleKeyInfo keyInfo;

            do
            {
                PrintMenuPPal();
                keyInfo = Console.ReadKey();
                SelectMenu(keyInfo, kvService);

            } while (keyInfo.Key != ConsoleKey.Escape && keyInfo.Key != ConsoleKey.D0);

        }

        private static void PrintMenuPPal()
        {
            Console.Clear();
            Console.WriteLine("KeyVault Secret Utility");
            Console.WriteLine("---------------------------");

            Console.WriteLine("1 ........ List of Secrets (List)");
            Console.WriteLine("2 ........ Get a Secret (Get)");
            Console.WriteLine("3 ........ Insert or Update Secret (Set)");
            Console.WriteLine("4 ........ Delete Secret (Delete)");

            Console.WriteLine("\r\n");
            Console.WriteLine("0 ........ Exit");
        }

        private static void PrintListItem(List<string> items)
        {
            Console.Clear();
            Console.WriteLine(" List of Secrets ");
            Console.WriteLine("-------------------------- ");

            foreach (var item in items)
            {
                Console.WriteLine("Id Secret: " + item);
            }

            Console.WriteLine("\r\nPress any key to back to main menu");
            Console.ReadKey();
        }


        private static void SelectMenu(ConsoleKeyInfo keyInfo, KeyVault kvService)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.D1: {
                        var r = kvService.ListSecretValues();
                        PrintListItem(r);
                        break;
                    }
                case ConsoleKey.D2:
                    {
                        Console.Clear();
                        Console.WriteLine(" Get a Secret ");
                        Console.WriteLine("-------------------------- ");
                        Console.Write(" Secret Id: ");
                        var secretId = Console.ReadLine();

                        var r = kvService.GetSecret(secretId);

                        Console.WriteLine(" The secret is: " + r.value + "\r\n");

                        Console.WriteLine("\r\nPress any key to back to main menu");
                        Console.ReadKey();
                        break;
                    }
                case ConsoleKey.D3:
                    {
                        Console.Clear();
                        Console.WriteLine(" Insert or Update Secret ");
                        Console.WriteLine("-------------------------- ");
                        Console.Write(" Secret Id: ");
                        var secretId = Console.ReadLine();

                        Console.Write(" Value: ");
                        var value = Console.ReadLine();

                        Console.Write(" Do you want insert or update this? (Y/N) ");
                        var keyI = Console.ReadKey();
                        if (keyI.Key == ConsoleKey.Y)
                        {
                            var r = kvService.SetSecret(secretId, value);

                            Console.WriteLine("\r\n\r\n The secret: " + secretId + " has been insert or update\r\n");
                        }

                        Console.WriteLine("\r\nPress any key to back to main menu");
                        Console.ReadKey();
                        break;
                    }
                case ConsoleKey.D4:
                    {
                        Console.Clear();
                        Console.WriteLine(" Delete Secret ");
                        Console.WriteLine("-------------------------- ");
                        Console.Write(" Secret Id: ");
                        var secretId = Console.ReadLine();

                        Console.Write(" Do you want to delete " +secretId+ "? (Y/N) ");
                        var keyI = Console.ReadKey();
                        if (keyI.Key == ConsoleKey.Y)
                        {
                            var r = kvService.DeleteSecret(secretId);
                            Console.WriteLine("\r\n\r\n The secret : " + secretId + " has been delete.\r\n");
                        }

                        Console.WriteLine("\r\nPress any key to back to main menu");
                        Console.ReadKey();
                        break;
                    }
            }
        }
    

    
    
    
    }
}
