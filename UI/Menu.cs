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
        Console.WriteLine("Type 'help' to see available commands.");
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
                        Console.WriteLine("Goodbye!");
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
                        Console.WriteLine("Data reloaded.");
                        break;
                    case "add":
                        HandleAdd();
                        break;
                    case "save":
                        _service.SaveChanges();
                        Console.WriteLine("Data saved to file.");
                        break;
                    default:
                        Console.WriteLine("Unknown command. Type 'help'.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }

    private void PrintHelp()
    {
        Console.WriteLine("Commands:");
        Console.WriteLine("  help                          - Show this help");
        Console.WriteLine("  list [order] [asc|desc]       - Show all products. order: name|price|category|id (default: name asc)");
        Console.WriteLine("     Example: list price desc");
        Console.WriteLine("  search name <text>            - Search by name (partial, case-insensitive)");
        Console.WriteLine("  search id <ID>                - Search by exact ID");
        Console.WriteLine("  search category <category>    - Search by category");
        Console.WriteLine("  search price <min> <max>      - Search by price range (e.g. search price 10 200)");
        Console.WriteLine("  show <ID>                     - Show full product details by ID");
        Console.WriteLine("  add                           - Add a new product (interactive)");
        Console.WriteLine("  save                          - Save current products to inventory.txt");
        Console.WriteLine("  reload                        - Reload data from file");
        Console.WriteLine("  exit                          - Exit");
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
            Console.WriteLine("Not enough parameters for search. Type 'help'.");
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
                if (p == null) Console.WriteLine("Product not found");
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
                    Console.WriteLine("Invalid parameters. Example: search price 10 200");
                    return;
                }
                if (!decimal.TryParse(parts[2].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var min) ||
                    !decimal.TryParse(parts[3].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var max))
                {
                    Console.WriteLine("Cannot parse numbers. Use format: 10 200");
                    return;
                }
                var byprice = _service.SearchByPriceRange(min, max);
                PrintProducts(byprice);
                break;
            default:
                Console.WriteLine("Unknown search criterion. Type 'help'.");
                break;
        }
    }

    private void HandleShow(string[] parts)
    {
        if (parts.Length < 2) { Console.WriteLine("usage: show <ID>"); return; }
        var id = parts[1];
        var p = _service.GetById(id);
        if (p == null) Console.WriteLine("Product not found");
        else Console.WriteLine(p);
    }

    private void HandleAdd()
    {
        Console.WriteLine("Add new product. Leave an input empty to cancel.");

        // ID
        Console.Write("ID: ");
        var id = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(id)) { Console.WriteLine("Cancelled."); return; }

        // Name
        Console.Write("Name: ");
        var name = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(name)) { Console.WriteLine("Cancelled."); return; }

        // Category
        Console.Write("Category: ");
        var category = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(category)) category = "";

        // Price
        decimal price = 0m;
        while (true)
        {
            Console.Write("Price (e.g. 199.99 or 199,99): ");
            var priceStr = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(priceStr)) { Console.WriteLine("Cancelled."); return; }
            if (decimal.TryParse(priceStr.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out price)) break;
            Console.WriteLine("Invalid price format. Try again.");
        }

        // Quantity
        int qty = 0;
        while (true)
        {
            Console.Write("Quantity (integer): ");
            var qStr = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(qStr)) { Console.WriteLine("Cancelled."); return; }
            if (int.TryParse(qStr, out qty)) break;
            Console.WriteLine("Invalid quantity. Try again.");
        }

        // Description
        Console.Write("Description: ");
        var desc = Console.ReadLine() ?? "";

        var product = new Product
        {
            Id = id,
            Name = name,
            Category = category,
            Price = price,
            Quantity = qty,
            Description = desc
        };

        try
        {
            _service.AddProduct(product);
            Console.WriteLine("Product added to memory. Use 'save' to write to file.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to add product: " + ex.Message);
        }
    }

    private void PrintProducts(System.Collections.Generic.IEnumerable<Product> items)
    {
        var list = new System.Collections.Generic.List<Product>(items);
        if (list.Count == 0)
        {
            Console.WriteLine("No results.");
            return;
        }
        Console.WriteLine($"Found: {list.Count}");
        foreach (var p in list)
        {
            Console.WriteLine(p.ToString());
        }
    }

    // Split arguments (supports quoted groups)
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