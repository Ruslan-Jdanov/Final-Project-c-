using System;
using System.Collections.Generic;
using System.Linq;

public class ProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo)
    {
        _repo = repo;
    }

    public IEnumerable<Product> GetAll(string orderBy = "name", bool asc = true)
    {
        var all = _repo.GetAll();
        return Sort(all, orderBy, asc);
    }

    private IEnumerable<Product> Sort(IEnumerable<Product> items, string orderBy, bool asc)
    {
        orderBy = orderBy?.ToLowerInvariant() ?? "name";
        IOrderedEnumerable<Product> ordered = orderBy switch
        {
            "price" => asc ? items.OrderBy(p => p.Price) : items.OrderByDescending(p => p.Price),
            "category" => asc ? items.OrderBy(p => p.Category) : items.OrderByDescending(p => p.Category),
            "id" => asc ? items.OrderBy(p => p.Id) : items.OrderByDescending(p => p.Id),
            _ => asc ? items.OrderBy(p => p.Name) : items.OrderByDescending(p => p.Name),
        };
        return ordered;
    }

    public Product? GetById(string id) => _repo.GetById(id);

    public IEnumerable<Product> SearchByName(string term)
    {
        if (string.IsNullOrWhiteSpace(term)) return Enumerable.Empty<Product>();
        term = term.Trim();
        return _repo.FindByPredicate(p => p.Name.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0);
    }

    public IEnumerable<Product> SearchByCategory(string category)
    {
        if (string.IsNullOrWhiteSpace(category)) return Enumerable.Empty<Product>();
        category = category.Trim();
        return _repo.FindByPredicate(p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<Product> SearchByPriceRange(decimal min, decimal max)
    {
        return _repo.FindByPredicate(p => p.Price >= min && p.Price <= max);
    }

    public IEnumerable<Product> Find(Func<Product, bool> predicate) => _repo.FindByPredicate(predicate);

    public void ReloadData() => _repo.Reload();

    // Новые методы для добавления и сохранения
    public void AddProduct(Product p) => _repo.Add(p);
    public void SaveChanges() => _repo.Save();
}