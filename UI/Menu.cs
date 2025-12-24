using System;
using System.Globalization;

public class Menu
{
    private readonly ProductService _service;

    public Menu(ProductService service)
    {
        _service = service;
    }

    public void ShowWelcome()
    {
        Console.WriteLine("Введите 'help' для списка команд.");
    }

    public void Run()
    {
        while (true)
        {
            Console.Write("\n> ");
            var input = Console.ReadLine();
            if (input == null) continue;
            var parts = SplitArgs(input);
            if (parts.Length == 0) continue;

            var cmd = parts[0].ToLowerInvariant();
            try
            {
                switch (cmd)
                {
                    case "help":
                        PrintHelp();
                        break;
                    case "exit":
                        Console.WriteLine("Выход. До встречи!");
                        return;
                    case "list":
                        HandleList(parts);
                        break;
                    case "search":
                        HandleSearch(parts);
                        break;
                    case "show":
                        HandleShow(parts);
                        break;
                    case "reload":
                        _service.ReloadData();
                        Console.WriteLine("Данные перезагружены.");
                        break;
                    default:
                        Console.WriteLine("Неизвестная команда. Введите 'help'.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }
    }

    private void PrintHelp()
    {
        Console.WriteLine("Команды:");
        Console.WriteLine("  help                          - Показать помощь");
        Console.WriteLine("  list [order] [asc|desc]       - Показать все товары. order: name|price|category|id (по умолчанию name asc)");
        Console.WriteLine("     Пример: list price desc");
        Console.WriteLine("  search name <текст>           - Поиск по имени (частичное совпадение)");
        Console.WriteLine("  search id <ID>                - Поиск по ID (точное совпадение)");
        Console.WriteLine("  search category <категория>   - Поиск по категории");
        Console.WriteLine("  search price <min> <max>      - Поиск по диапазону цен (например: search price 10 200)");
        Console.WriteLine("  show <ID>                     - Показать полные данные товара по ID");
        Console.WriteLine("  reload                        - Перезагрузить данные из файла");
        Console.WriteLine("  exit                          - Выйти");
    }

    private void HandleList(string[] parts)
    {
        string order = "name";
        bool asc = true;
        if (parts.Length >= 2) order = parts[1];
        if (parts.Length >= 3) asc = parts[2].ToLowerInvariant() != "desc";

        var items = _service.GetAll(order, asc);
        PrintProducts(items);
    }

    private void HandleSearch(string[] parts)
    {
        if (parts.Length < 3)
        {
            Console.WriteLine("Недостаточно параметров для search. Введите 'help'.");
            return;
        }

        var by = parts[1].ToLowerInvariant();
        switch (by)
        {
            case "name":
                var term = string.Join(' ', parts, 2, parts.Length - 2);
                var found = _service.SearchByName(term);
                PrintProducts(found);
                break;
            case "id":
                var id = parts[2];
                var p = _service.GetById(id);
                if (p == null) Console.WriteLine("Товар не найден");
                else Console.WriteLine(p);
                break;
            case "category":
                var cat = string.Join(' ', parts, 2, parts.Length - 2);
                var bycat = _service.SearchByCategory(cat);
                PrintProducts(bycat);
                break;
            case "price":
                if (parts.Length < 4)
                {
                    Console.WriteLine("Неверные параметры. Пример: search price 10 200");
                    return;
                }
                if (!decimal.TryParse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out var min) ||
                    !decimal.TryParse(parts[3], NumberStyles.Any, CultureInfo.InvariantCulture, out var max))
                {
                    Console.WriteLine("Невозможно распознать числа. Используйте формат: 10 200");
                    return;
                }
                var byprice = _service.SearchByPriceRange(min, max);
                PrintProducts(byprice);
                break;
            default:
                Console.WriteLine("Неизвестный критерий поиска. Введите 'help'.");
                break;
        }
    }

    private void HandleShow(string[] parts)
    {
        if (parts.Length < 2) { Console.WriteLine("usage: show <ID>"); return; }
        var id = parts[1];
        var p = _service.GetById(id);
        if (p == null) Console.WriteLine("Товар не найден");
        else Console.WriteLine(p);
    }

    private void PrintProducts(System.Collections.Generic.IEnumerable<Product> items)
    {
        var list = new System.Collections.Generic.List<Product>(items);
        if (list.Count == 0)
        {
            Console.WriteLine("Ничего не найдено.");
            return;
        }
        Console.WriteLine($"Найдено: {list.Count}");
        foreach (var p in list)
        {
            Console.WriteLine(p.ToString());
        }
    }

    // Простейший разбор аргументов — сохраняем группы в кавычках как один аргумент
    private string[] SplitArgs(string input)
    {
        var args = new System.Collections.Generic.List<string>();
        bool inQuote = false;
        var current = new System.Text.StringBuilder();
        foreach (var ch in input)
        {
            if (ch == '"')
            {
                inQuote = !inQuote;
                continue;
            }
            if (char.IsWhiteSpace(ch) && !inQuote)
            {
                if (current.Length > 0)
                {
                    args.Add(current.ToString());
                    current.Clear();
                }
            }
            else current.Append(ch);
        }
        if (current.Length > 0) args.Add(current.ToString());
        return args.ToArray();
    }
}