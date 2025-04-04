using System;
using System.Collections.Generic;
using Microsoft.SemanticKernel;

namespace Plugins;

public class Product
{
    public string Id { get; set; }
    public string Name { get; set; }

    public Product(string id, string name)
    {
        Id = id;
        Name = name;
    }
}

public class ECommercePlugins
{
    private List<Product> cart;
    private List<string> orders;
    private List<string> payments;
    private List<string> refunds;

    public ECommercePlugins()
    {
        cart = new List<Product>();
        orders = new List<string>();
        payments = new List<string>();
        refunds = new List<string>();
    }

    [KernelFunction]
    public List<Product> GetProducts()
    {
        return new List<Product>
            {
                new Product("1", "Product 1"),
                new Product("2", "Product 2"),
                new Product("3", "Product 3"),
                new Product("4", "Product 4"),
                new Product("5", "Product 5")
            };
    }

    [KernelFunction]
    public Product GetProduct(string productId)
    {
        return new Product(productId, $"Product {productId}");
    }

    [KernelFunction]
    public void AddToCart(Product product)
    {
        cart.Add(product);
        Console.WriteLine($"{product.Name} added to cart.");
    }

    [KernelFunction]
    public void RemoveFromCart(string productId)
    {
        var product = cart.Find(p => p.Id == productId);
        if (product != null)
        {
            cart.Remove(product);
            Console.WriteLine($"{product.Name} removed from cart.");
        }
        else
        {
            Console.WriteLine($"Product with ID {productId} not found in cart.");
        }
    }

    [KernelFunction]
    public void CreateOrder()
    {
        if (cart.Count > 0)
        {
            string order = string.Join(", ", cart.Select(p => p.Name));
            orders.Add(order);
            cart.Clear();
            Console.WriteLine($"Order created: {order}");
        }
        else
        {
            Console.WriteLine("Cart is empty. Cannot create order.");
        }
    }

    [KernelFunction]
    public void PayOrder(int orderId)
    {
        if (orderId >= 0 && orderId < orders.Count)
        {
            string order = orders[orderId];
            payments.Add(order);
            Console.WriteLine($"Order {orderId} paid: {order}");
        }
        else
        {
            Console.WriteLine($"Order {orderId} not found.");
        }
    }

    [KernelFunction]
    public void RefundOrder(int orderId)
    {
        if (orderId >= 0 && orderId < payments.Count)
        {
            string order = payments[orderId];
            refunds.Add(order);
            Console.WriteLine($"Order {orderId} refunded: {order}");
        }
        else
        {
            Console.WriteLine($"Order {orderId} not found or not paid.");
        }
    }
}
