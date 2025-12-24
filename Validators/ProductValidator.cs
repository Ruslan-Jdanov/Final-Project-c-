using System;

public static class ProductValidator
{
    public static void Validate(Product p)
    {
        if (p == null) throw new ArgumentNullException(nameof(p));
        if (string.IsNullOrWhiteSpace(p.Id)) throw new ArgumentException("Id is required.");
        if (string.IsNullOrWhiteSpace(p.Name)) throw new ArgumentException("Name is required.");
        if (p.Price < 0) throw new ArgumentException("Price cannot be negative.");
        if (p.Quantity < 0) throw new ArgumentException("Quantity cannot be negative.");
    }
}