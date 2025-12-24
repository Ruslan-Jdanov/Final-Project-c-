using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class ProductServiceTests
{
    [Fact]
    public void GetAll_Sorting_Works()
    {
        var products = new List<Product>
        {
            new Product { Id="A", Name="A", Category="X", Price=10m, Quantity=1 },
            new Product { Id="B", Name="B", Category="X", Price=5m, Quantity=1 },
            new Product { Id="C", Name="C", Category="Y", Price=7m, Quantity=1 }
        };
        var repo = new InMemoryRepo(products);
        var svc = new ProductService(repo);

        var byPriceDesc = svc.GetAll("price", asc: false).ToList();
        Assert.Equal(new[] { "A", "C", "B" }, byPriceDesc.Select(p => p.Id));
    }

    [Fact]
    public void SearchByName_Finds_Partial()
    {
        var products = new List<Product>
        {
            new Product { Id="1", Name="Gaming Laptop", Category="Laptop", Price=1, Quantity=1 },
            new Product { Id="2", Name="Office Laptop", Category="Laptop", Price=1, Quantity=1 },
        };
        var repo = new InMemoryRepo(products);
        var svc = new ProductService(repo);

        var found = svc.SearchByName("gaming").ToList();
        Assert.Single(found);
        Assert.Equal("1", found[0].Id);
    }

    [Fact]
    public void SearchByCategory_Works()
    {
        var products = new List<Product>
        {
            new Product { Id="1", Name="A", Category="X", Price=1, Quantity=1 },
            new Product { Id="2", Name="B", Category="Y", Price=1, Quantity=1 }
        };
        var repo = new InMemoryRepo(products);
        var svc = new ProductService(repo);

        var found = svc.SearchByCategory("x").ToList();
        Assert.Single(found);
        Assert.Equal("1", found[0].Id);
    }

    [Fact]
    public void SearchByPriceRange_Works()
    {
        var products = new List<Product>
        {
            new Product { Id="1", Name="A", Category="X", Price=10, Quantity=1 },
            new Product { Id="2", Name="B", Category="X", Price=20, Quantity=1 }
        };
        var repo = new InMemoryRepo(products);
        var svc = new ProductService(repo);

        var found = svc.SearchByPriceRange(5m, 15m).ToList();
        Assert.Single(found);
        Assert.Equal("1", found[0].Id);
    }

    private class InMemoryRepo : IProductRepository
    {
        private readonly List<Product> _items;
        public InMemoryRepo(List<Product> items) { _items = items.ToList(); }
        public IEnumerable<Product> GetAll() => _items;
        public Product? GetById(string id) => _items.FirstOrDefault(x => x.Id == id);
        public IEnumerable<Product> FindByPredicate(Func<Product, bool> predicate) => _items.Where(predicate);
        public void Reload() { /*noop*/ }
        public void Add(Product product) { _items.Add(product); }
        public void Save() { /*noop*/ }
    }
}