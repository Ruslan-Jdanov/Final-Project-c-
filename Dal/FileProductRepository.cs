using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text;

// File format: ID|Name|Category|Price|Quantity|Description
public class FileProductRepository : IProductRepository
{
    private readonly string _path;
    private List<Product> _cache = new List<Product>();

    public FileProductRepository(string path)
    {
        _path = path;
        LoadFromFile();
    }

    // Load file lines into _cache
    private void LoadFromFile()
    {
        _cache.Clear();
        if (!File.Exists(_path))
        {
            Console.WriteLine("Data file not found: " + _path);
            return;
        }

        var lines = File.ReadAllLines(_path);
        int lineNo = 0;
        foreach (var raw in lines)
        {
            lineNo++;
            var line = raw.Trim();
            if (string.IsNullOrEmpty(line) || line.StartsWith("#")) continue; // allow comments with '#'
            var parts = line.Split('|');
            if (parts.Length < 6)
            {
                Console.WriteLine($"Malformed line (line {lineNo}): {line}");
                continue;
            }

            var p = new Product();
            p.Id = parts[0].Trim();
            p.Name = parts[1].Trim();
            p.Category = parts[2].Trim();

            // Support both comma and dot decimal separators:
            var priceStr = parts[3].Trim().Replace(',', '.');
            if (!decimal.TryParse(priceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
            {
                Console.WriteLine($"Price parse error at line {lineNo}: '{parts[3]}'");
                continue;
            }
            p.Price = price;

            if (!int.TryParse(parts[4].Trim(), out var qty))
            {
                Console.WriteLine($"Quantity parse error at line {lineNo}: '{parts[4]}'");
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

    public void Add(Product product)
    {
        // Basic validation: require Id and Name
        if (string.IsNullOrWhiteSpace(product.Id) || string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Product must have Id and Name.");

        // Ensure unique ID
        var existing = GetById(product.Id);
        if (existing != null)
            throw new InvalidOperationException($"A product with ID '{product.Id}' already exists.");

        _cache.Add(product);
    }

    public void Save()
    {
        // Ensure directory exists
        var dir = Path.GetDirectoryName(_path);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        var lines = new List<string>();
        foreach (var p in _cache)
        {
            // Sanitize fields so '|' will not break the format
            string id = (p.Id ?? "").Replace("|", " ").Trim();
            string name = (p.Name ?? "").Replace("|", " ").Trim();
            string cat = (p.Category ?? "").Replace("|", " ").Trim();
            string desc = (p.Description ?? "").Replace("|", " ").Trim();

            // Price in invariant culture (use '.' as decimal separator)
            string price = p.Price.ToString(CultureInfo.InvariantCulture);
            string qty = p.Quantity.ToString();

            lines.Add($"{id}|{name}|{cat}|{price}|{qty}|{desc}");
        }

        File.WriteAllLines(_path, lines, Encoding.UTF8);
    }
}