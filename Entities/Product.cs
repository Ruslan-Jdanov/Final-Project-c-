using System;

public class Product
{
    // Простейшая сущность продукта
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public decimal Price { get; set; } = 0m;
    public int Quantity { get; set; } = 0;
    public string Description { get; set; } = "";

    public override string ToString()
    {
        return $"{Id} | {Name} | {Category} | {Price:C} | qty: {Quantity}\n  {Description}";
    }
}