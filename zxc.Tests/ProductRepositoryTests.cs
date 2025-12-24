using System;
using System.IO;
using System.Linq;
using Xunit;
using System.Collections.Generic;

public class ProductRepositoryTests
{
    private string CreateTempInventory(string content)
    {
        var tmp = Path.GetTempFileName();
        File.WriteAllText(tmp, content);
        return tmp;
    }

    private string SampleInventory => @"
# Format: ID|Name|Category|Price|Quantity|Description
CE-001|Gaming Laptop X15|Laptop|1499.99|10|High-performance gaming laptop with RTX.
CE-002|Office Laptop A14|Laptop|599.00|25|Lightweight laptop for office tasks.
CE-003|Mechanical Keyboard MK-1|Accessory|79.50|100|RGB mechanical keyboard.
CE-004|Wireless Mouse WM-200|Accessory|29.99|200|Ergonomic wireless mouse.
";

    [Fact]
    public void Load_From_File_Parses_Products()
    {
        var path = CreateTempInventory(SampleInventory);
        try
        {
            var repo = new FileProductRepository(path);
            var all = repo.GetAll().ToList();
            Assert.Equal(4, all.Count);
            Assert.Contains(all, p => p.Id == "CE-001" && p.Name.Contains("Gaming Laptop"));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void SearchByName_Returns_Matching_Products()
    {
        var path = CreateTempInventory(SampleInventory);
        try
        {
            var repo = new FileProductRepository(path);
            var service = new ProductService(repo);
            var found = service.SearchByName("mouse").ToList();
            Assert.Single(found);
            Assert.Equal("CE-004", found[0].Id);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Add_DuplicateId_Throws()
    {
        var path = CreateTempInventory(SampleInventory);
        try
        {
            var repo = new FileProductRepository(path);
            var p = new Product { Id = "CE-001", Name = "Dup", Category = "X", Price = 1, Quantity = 1, Description = "" };
            Assert.Throws<InvalidOperationException>(() => repo.Add(p));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void SearchByPriceRange_Works()
    {
        var path = CreateTempInventory(SampleInventory);
        try
        {
            var repo = new FileProductRepository(path);
            var service = new ProductService(repo);
            var found = service.SearchByPriceRange(20m, 100m).ToList();
            Assert.Equal(2, found.Count); // Keyboard 79.50 and Mouse 29.99
            var ids = new HashSet<string>(found.Select(x => x.Id));
            Assert.Contains("CE-003", ids);
            Assert.Contains("CE-004", ids);
        }
        finally
        {
            File.Delete(path);
        }
    }
}