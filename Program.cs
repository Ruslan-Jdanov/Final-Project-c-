using System;
using System.IO;

class Program
{
    // Настройте эти данные под себя (имя приложения, версия, дата создания, разработчик)
    const string APP_NAME = "Computer Equipment Warehouse";
    const string VERSION = "1.0";
    const string CREATION_DATE = "2025-12-24";
    const string DEVELOPER_NAME = "rusyajdanowww-cmyk";
    const string DEVELOPER_EMAIL = "rusyajdanowww-cmyk@example.com";

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine($"{APP_NAME} — версия {VERSION} (создано {CREATION_DATE})");
        Console.WriteLine($"Разработчик: {DEVELOPER_NAME} <{DEVELOPER_EMAIL}>");
        Console.WriteLine();

        // Путь к файлу inventory.txt — ищем рядом с папкой приложения
        var dataFile = Path.Combine(AppContext.BaseDirectory, "inventory.txt");
        if (!File.Exists(dataFile))
        {
            // Также допускаем, что пользователь запустил из исходников — проверим в рабочей директории
            var alt = Path.Combine(Directory.GetCurrentDirectory(), "inventory.txt");
            if (File.Exists(alt)) dataFile = alt;
        }

        Console.WriteLine($"Загружаем данные из: {dataFile}");
        var repository = new FileProductRepository(dataFile);
        var service = new ProductService(repository);
        var menu = new Menu(service);

        Console.WriteLine();
        menu.ShowWelcome();
        menu.Run();
    }
}