﻿using System;
using System.IO;

class Program
{
    const string APP_NAME = "Computer Equipment Warehouse";
    const string VERSION = "1.0";
    const string CREATION_DATE = "2025-12-24";
    const string DEVELOPER_NAME = "Ruslan Jdanov";
    const string DEVELOPER_EMAIL = "Ruslan_Jdanov@student.itpu.uz";

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine($"{APP_NAME} — version {VERSION} (created {CREATION_DATE})");
        Console.WriteLine($"Developer: {DEVELOPER_NAME} <{DEVELOPER_EMAIL}>");
        Console.WriteLine();

        var dataFile = Path.Combine(AppContext.BaseDirectory, "inventory.txt");
        if (!File.Exists(dataFile))
        {
            var alt = Path.Combine(Directory.GetCurrentDirectory(), "inventory.txt");
            if (File.Exists(alt)) dataFile = alt;
        }

        Console.WriteLine($"Loading data from: {dataFile}");

        // Simple console logger (no external packages required)
        var logger = new ConsoleAppLogger();

        var repository = new FileProductRepository(dataFile, logger);
        var service = new ProductService(repository);
        var menu = new Menu(service);

        Console.WriteLine();
        menu.ShowWelcome();
        menu.Run();
    }
}