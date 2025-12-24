using System.Collections.Generic;

public interface IProductRepository
{
    IEnumerable<Product> GetAll();
    Product? GetById(string id);
    IEnumerable<Product> FindByPredicate(System.Func<Product, bool> predicate);
    void Reload();    // Reload data from the source (file)
    void Add(Product product);
    void Save();      // Save current cache back to the source (file)
}