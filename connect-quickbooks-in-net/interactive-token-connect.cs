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