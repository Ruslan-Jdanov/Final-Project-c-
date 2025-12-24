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
    private readonly IAppLogger? _logger;

    // Backwards-compatible constructor: logger optional
    public FileProductRepository(string path, IAppLogger? logger = null)
    {
        _path = path;
        _logger = logger;
        LoadFromFile();
    }

    // Load file lines into _cache
    private void LoadFromFile()
    {
        _cache.Clear();
        if (!File.Exists(_path))
        {
            Log($"Data file not found: {_path}", isError: true);
            return;
        }

        var lines = File.ReadAllLines(_path);
        int lineNo = 0;
        foreach (var raw in lines)
        {
            lineNo++;
            var line = raw.Trim();
            if (string.IsNullOrEmpty(line)) continue;

            // Allow comments starting with '#'
            if (line.StartsWith("#")) continue;

            var parts = line.Split('|').Select(p => p.Trim()).ToArray();

            // Handle lines that start with a numeric index like "1| CE-001|..."
            // If first part is numeric and there are at least 7 parts, drop the numeric column
            if (parts.Length >= 7 && int.TryParse(parts[0], out _))
            {
                parts = parts.Skip(1).ToArray();
            }

            // Also if a line like "1| # Format: ..." (number + comment), skip it
            if (parts.Length > 0 && parts[0].StartsWith("#")) continue;

            if (parts.Length < 6)
            {
                Log($"Malformed line (line {lineNo}): {line}", isError: true);
                continue;
            }

            var p = new Product();
            p.Id = parts[0];
            p.Name = parts[1];
            p.Category = parts[2];

            // Support both comma and dot decimal separators:
            var priceStr = parts[3].Replace(',', '.');
            if (!decimal.TryParse(priceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
            {
                Log($"Price parse error at line {lineNo}: '{parts[3]}'", isError: true);
                continue;
            }
            p.Price = price;

            if (!int.TryParse(parts[4], out var qty))
            {
                Log($"Quantity parse error at line {lineNo}: '{parts[4]}'", isError: true);
                continue;
            }
            p.Quantity = qty;

            p.Description = parts[5];
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
        // Central validation (uses ProductValidator)
        try
        {
            ProductValidator.Validate(product);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Product validation failed: " + ex.Message, ex);
        }

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
        Log($"Saved {_cache.Count} products to {_path}", isError: false);
    }

    private void Log(string message, bool isError)
    {
        if (_logger != null)
        {
            if (isError) _logger.LogError(message); else _logger.LogInfo(message);
        }
        else
        {
            // Backwards-compatible fallback
            if (isError) Console.WriteLine("ERROR: " + message);
            else Console.WriteLine(message);
        }
    }
}