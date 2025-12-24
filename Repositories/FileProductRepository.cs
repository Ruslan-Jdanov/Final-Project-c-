using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Формат строки в файле inventory.txt:
// ID|Name|Category|Price|Quantity|Description
public class FileProductRepository : IProductRepository
{
    private readonly string _path;
    private List<Product> _cache = new List<Product>();

    public FileProductRepository(string path)
    {
        _path = path;
        LoadFromFile();
    }

    private void LoadFromFile()
    {
        _cache.Clear();
        if (!File.Exists(_path))
        {
            Console.WriteLine("Файл данных не найден: " + _path);
            return;
        }

        var lines = File.ReadAllLines(_path);
        int lineNo = 0;
        foreach (var raw in lines)
        {
            lineNo++;
            var line = raw.Trim();
            if (string.IsNullOrEmpty(line) || line.StartsWith("#")) continue; // комментарии начинаются с #
            var parts = line.Split('|');
            if (parts.Length < 6)
            {
                Console.WriteLine($"Неправильная строка (line {lineNo}): {line}");
                continue;
            }

            var p = new Product();
            p.Id = parts[0].Trim();
            p.Name = parts[1].Trim();
            p.Category = parts[2].Trim();

            if (!decimal.TryParse(parts[3].Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var price))
            {
                Console.WriteLine($"Ошибка разбора цены в строке {lineNo}: '{parts[3]}'");
                continue;
            }
            p.Price = price;

            if (!int.TryParse(parts[4].Trim(), out var qty))
            {
                Console.WriteLine($"Ошибка разбора количества в строке {lineNo}: '{parts[4]}'");
                continue;
            }
            p.Quantity = qty;

            p.Description = parts[5].Trim();
            _cache.Add(p);
        }
    }

    public IEnumerable<Product> GetAll()
    {
        return _cache.ToList();
    }

    public Product? GetById(string id)
    {
        return _cache.FirstOrDefault(x => string.Equals(x.Id, id, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<Product> FindByPredicate(Func<Product, bool> predicate)
    {
        return _cache.Where(predicate).ToList();
    }

    public void Reload()
    {
        LoadFromFile();
    }
}