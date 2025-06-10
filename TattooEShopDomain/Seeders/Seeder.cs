using TattooEShopDomain.Models;
using TattooEShopDomain.Repositories;

namespace TattooEShopDomain.Seeders;

public class Seeder (DataContext context) : ISeeder
{
    public async Task Seed()
    {
        if (!context.Products.Any())
        {
            var products = new List<Product>
                {
                    new Product { Id = 1, Name = "RL - 0.30mm - LT - Mast Pro Cartridge", Ean = "194675", Price = 5.90m, Stock = 50, Sku = "RLLTMPC"},
                    new Product { Id = 2, Name = "Radiant Geneva Real Black - tattoo ink", Ean = "146701", Price = 89, Stock = 10, Sku = "RGRBTI"},
                    new Product { Id = 3, Name = "Silikonowa skóra do ćwiczeń", Ean = "126489", Price = 24.90m, Stock = 5, Sku = "SSDC"},
                };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
