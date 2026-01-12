# How to connect to QucikBooks Online in .NET with C#

Based on [https://www.devart.com/dotconnect/quickbooks/connect-to-quickbooks-online.html](https://www.devart.com/dotconnect/quickbooks/connect-to-quickbooks-online.html)

This tutorial demonstrates how to integrate QucikBooks Online with your .NET application using C#. Whether you're performing direct ADO.NET operations or building an object-relational model with EF Core, dotConnect for QucikBooks Online provides a seamless, secure, and high-performance experience. We'll cover basic connections, credential management, and ORM-based development.

### Installation
------------

1. Install the NuGet Package

```sh
Install-Package Devart.Data.QuickBooks
```
2. Activate license

* **Free Trial License:** Evaluate the full capabilities of dotConnect for QucikBooks Online in a non-commercial environment—ideal for development and testing. [Start your free trial](https://secure.devart.com/licenses?product=dotconnect/quickbooks&source=activation)
* **Commercial License:** Deploy dotConnect for QucikBooks Online in commercial applications and access full technical support with a valid license. [Purchase a license](https://www.devart.com/dotconnect/quickbooks/ordering.html)

## Connect to QucikBooks Online using C#

This section guides you through connecting to QucikBooks Online using dotConnect for QucikBooks Online and ADO.NET. You'll create a connection string, authenticate your session, and use ADO.NET classes like QucikbooksConnection and QucikbooksCommand to query data from your QucikBooks Online organization.

```cs
using Devart.Data.QuickBooks;

namespace QuickBooksConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Authentication Type=OAuthInteractive;" +
                                    "License key=**********;";

            try
            {
                using (QuickBooksConnection quickBooksConnection = new QuickBooksConnection())
                {
                    quickBooksConnection.ConnectionString = connectionString;
                    quickBooksConnection.Open();

                    Console.WriteLine("Connected to QuickBooks Online successfully.");

                    quickBooksConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
    }
}
```

## Connect to QucikBooks Online using existing credentials

If you already have QucikBooks Online credentials or an OAuth token, this section shows how to securely reuse them in your connection string. You'll also learn how to manage session expiration and refresh tokens for long-lived integrations.

```cs
using Devart.Data.QuickBooks;

namespace QuickBooksConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            bool sandbox = true;
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";

            string connectionString = $"Authentication Type=OAuthInteractive;" +
                                      $"Sandbox={sandbox};" +
                                      $"Client Id={clientId};" +
                                      $"Client Secret={clientSecret};" +
                                      "License key=**********";

            try
            {
                using (QuickBooksConnection quickBooksConnection = new QuickBooksConnection(connectionString))
                {
                    quickBooksConnection.Open();
                    Console.WriteLine("Connected to QuickBooks Online successfully.");
                    quickBooksConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
```

## Connect to QucikBooks Online with EF Core

Leverage EF Core to simplify data access with strongly-typed models. You'll use LINQ to interact with QucikBooks Online data as .NET objects, simplifying queries and CRUD operations.

```cs
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
```