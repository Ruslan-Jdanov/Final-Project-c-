using System;
using System.IO;
using System.Linq;
using Xunit;
using System.Collections.Generic;

public class FileProductRepositoryTests
{
    private IAppLogger GetSilentLogger() => new TestSilentLogger();

    [Fact]
    public void Load_NormalFile_ParsesProducts()
    {
        var lines = new[]
        {
            "# Format: ID|Name|Category|Price|Quantity|Description",
            "CE-001|Gaming Laptop X15|Laptop|1499.99|10|High-performance",
            "CE-002|Office Laptop A14|Laptop|599.00|25|Lightweight"
        };
        var tmp = Path.GetTempFileName();
        File.WriteAllLines(tmp, lines);
        try
        {
            var repo = new FileProductRepository(tmp, GetSilentLogger());
            var all = repo.GetAll().ToList();
            Assert.Equal(2, all.Count);
            Assert.Equal("CE-001", all[0].Id);
        }
        finally { File.Delete(tmp); }
    }

    [Fact]
    public void Load_WithNumericPrefixes_ParsesProducts()
    {
        var lines = new[]
        {
            "1| # Format: ID|Name|Category|Price|Quantity|Description",
            "2| CE-001|Gaming Laptop X15|Laptop|1499.99|10|High-performance",
            "3| CE-002|Office Laptop A14|Laptop|599.00|25|Lightweight"
        };
        var tmp = Path.GetTempFileName();
        File.WriteAllLines(tmp, lines);
        try
        {
            var repo = new FileProductRepository(tmp, GetSilentLogger());
            var all = repo.GetAll().ToList();
            Assert.Equal(2, all.Count);
            Assert.Equal("CE-001", all[0].Id);
        }
        finally { File.Delete(tmp); }
    }

    private class TestSilentLogger : IAppLogger
    {
        public void LogError(string message) { }
        public void LogInfo(string message) { }
        public void LogWarning(string message) { }
    }
}