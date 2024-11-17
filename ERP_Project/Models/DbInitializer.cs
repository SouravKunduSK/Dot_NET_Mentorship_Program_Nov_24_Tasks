using Microsoft.EntityFrameworkCore;

namespace ERP_Project.Models
{
    public class DbInitializer
    {
        public static async Task SeedDefaultAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ERPDbContext>();

                try
                {
                    //Check Database is created
                    await context.Database.EnsureCreatedAsync();

                    //Check if Database is already seeded
                    if (context.tblProducts.Any() || context.tblOrders.Any())
                    {
                        return;
                    }

                    //Seed Products
                    var products = new List<Product>()
                {
                    new Product{Name="Widget A", UnitPrice=50, Stock=300},
                    new Product{Name="Gadget B", UnitPrice=75, Stock=150},
                    new Product{Name="Tool C", UnitPrice=100, Stock=200}
                };

                    await context.tblProducts.AddRangeAsync(products);
                    //Save Changes
                    await context.SaveChangesAsync();
                    // Retrieve Product IDs after seeding
                    var productA = await context.tblProducts.FirstOrDefaultAsync(p => p.Name == "Widget A");
                    var productB = await context.tblProducts.FirstOrDefaultAsync(p => p.Name == "Gadget B");
                    var productC = await context.tblProducts.FirstOrDefaultAsync(p => p.Name == "Tool C");

                    if (productA == null || productB == null || productC == null)
                        throw new Exception("Failed to retrieve seeded products.");
                    //Seed Orders
                    var orders = new List<Order>()
                {
                    new Order
                    {
                        
                        ProductId = 19,
                        CustomerName = "John Doe",
                        Quantity = 20,
                        OrderDate = new DateTime(2024, 1, 15, 14, 0, 0)
                    },
                    new Order
                    {
                        
                        ProductId = 20,
                        CustomerName = "Jane Smith",
                        Quantity = 10,
                        OrderDate = new DateTime(2024, 2, 10, 10, 30, 0)
                    },
                    new Order
                    {
                        
                        ProductId = 19,
                        CustomerName = "Sam Wilson",
                        Quantity = 15,
                        OrderDate = new DateTime(2024, 3, 5, 9, 15, 0)
                    }
                };
                    await context.tblOrders.AddRangeAsync(orders);

                    //Save Changes
                    await context.SaveChangesAsync();
                }

                catch (Exception ex)
                {
                    // Log the error (you can integrate a logging library here)
                    Console.WriteLine($"An error occurred during database seeding: {ex.Message}");
                }
            }
        }
    }
}
