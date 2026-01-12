using Microsoft.EntityFrameworkCore;
using Devart.Data.Quickbooks.EFCore;

namespace EfCoreQuickbooksDemo
{
    public class Customer
    {
        public string Id { get; set; } = "";
        public string? DisplayName { get; set; }
        public string? CompanyName { get; set; }
        public string? PrimaryEmailAddrAddress { get; set; }
    }

    public class QuickbooksContext : DbContext
    {
        private readonly string _connectionString;

        public QuickbooksContext(string connectionString) => _connectionString = connectionString;

        public DbSet<Customer> Customers => Set<Customer>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseQuickbooks(_connectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<Customer>();
            e.ToTable("Customer");
            e.HasKey(c => c.Id);
            e.Property(c => c.Id).HasColumnName("Id");
            e.Property(c => c.DisplayName).HasColumnName("DisplayName");
            e.Property(c => c.CompanyName).HasColumnName("CompanyName");
            e.Property(c => c.PrimaryEmailAddrAddress).HasColumnName("PrimaryEmailAddr.Address");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Fill in License Key
            var connectionString =
                "Authentication Type=OAuthInteractive;" +
                "License Key=**********";

            using (var context = new QuickbooksContext(connectionString))
            {
                try
                {
                    var customers = context.Customers
                        .Select(c => new
                        {
                            DisplayName = c.DisplayName,
                            CompanyName = c.CompanyName,
                            PrimaryEmailAddrAddress = c.PrimaryEmailAddrAddress
                        })
                        .Take(10) // Limit the results to 10 customers
                        .ToList();

                    foreach (var customer in customers)
                    {
                        Console.WriteLine($"Display Name: {customer.DisplayName}");
                        Console.WriteLine($"Company Name: {customer.CompanyName}");
                        Console.WriteLine($"Email: {customer.PrimaryEmailAddrAddress}");
                        Console.WriteLine(new string('-', 20));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            Console.WriteLine("\nDone. Press any key to exit...");
            Console.ReadKey();
        }
    }
}