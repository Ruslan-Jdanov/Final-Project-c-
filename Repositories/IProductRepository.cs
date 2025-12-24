using System.Collections.Generic;

public interface IProductRepository
{
    IEnumerable<Product> GetAll();
    Product? GetById(string id);
    IEnumerable<Product> FindByPredicate(System.Func<Product, bool> predicate);
    void Reload();
    void Add(Product product);
    void Save();
}