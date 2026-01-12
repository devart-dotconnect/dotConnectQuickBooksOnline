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