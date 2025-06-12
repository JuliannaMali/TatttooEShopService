using TattooEShopDomain.Models;
using TattooEShopDomain.Repositories;

namespace TattooEShopDomain.Seeders;

public class Seeder (DataContext context) : ISeeder
{
    public async Task Seed()
    {
        if (!context.Categories.Any())
        {
            var categories = new List<Category>
                {
                    new Category { Name = "Ink" },
                    new Category { Name = "Cartridges" },
                    new Category { Name = "Practice Materials" },
                };

            context.Categories.AddRange(categories);
            context.SaveChanges();
        }
        if (!context.Products.Any())
        {
            var ink = context.Categories.First(c => c.Name == "Ink");
            var cartridges = context.Categories.First(c => c.Name == "Cartridges");
            var practice = context.Categories.First(c => c.Name == "Practice Materials");

            var products = new List<Product>
            {
                new Product {
                    Name = "RL - 0.30mm - LT - Mast Pro Cartridge",
                    Ean = "194675",
                    Price = 5.90m,
                    Stock = 50,
                    Sku = "RLLTMPC",
                    CategoryId = cartridges.Id
                },
                new Product {
                    Name = "Radiant Geneva Real Black - tattoo ink",
                    Ean = "146701",
                    Price = 89,
                    Stock = 10,
                    Sku = "RGRBTI",
                    CategoryId = ink.Id
                },
                new Product {
                    Name = "Silikonowa skóra do ćwiczeń",
                    Ean = "126489",
                    Price = 24.90m,
                    Stock = 5,
                    Sku = "SSDC",
                    CategoryId = practice.Id
                },
            };
            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
