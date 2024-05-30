
using System.Globalization;

namespace testing_app;

class Program
{
    async static Task Main(string[] args)
    {
        MoreDateStuff();
        // RunRunner();
        // await HighLevelClientStuff();
        // AddingNumbersSequentially(1, 1000);
        
        // StringNullStuff();
        // OrderLineItemStuff();
        // DateStuff();
        // StringStuff();
    }

    private static void MoreDateStuff()
    {
        var dateString = "5/27/2024 11:55:00 PM -06:00";
        var date = DateTimeOffset.Parse(dateString, CultureInfo.InvariantCulture);
        Console.WriteLine(date.ToString("yyyy-MM-ddTHH:mm:ssZ"));
    }

    private static void RunRunner()
    {
        // Define the path to the file
        string filePath = @"/Users/prestonweight/Desktop/Main.go";

        // Run the AppendHelloWorld method indefinitely
        while (true)
        {
            AppendHelloWorld(filePath);
            // Wait for a random amount of time between 1 and 30 seconds
            Thread.Sleep(new Random().Next(1000, 30001));
        }
    }

    static void AppendHelloWorld(string filePath)
    {
        Console.WriteLine("Appending Hello World to the file...");
        // Ensure the file exists or create a new one
        if (!File.Exists(filePath))
        {
            using (StreamWriter sw = File.CreateText(filePath))
            {
                sw.WriteLine("package main");  // Initial setup for a Go file, if needed
                sw.WriteLine("import \"fmt\"");
                sw.WriteLine();
                sw.WriteLine("func main() {");
                sw.WriteLine("\tfmt.Println(\"Hello World\")");
                sw.WriteLine("}");
            }
        }
        else
        {
            // Append "Hello World" to the existing file
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine("\tfmt.Println(\"Hello World\")"); // Adding new line to the main function
            }
        }
    }

    private static async Task HighLevelClientStuff()
    {
        try
        {
            // var client = new HighLevelClient("GO_HIGH_LEVEL_API_KEY");
            // var request = new Connect.HighLevel.Models.HighLevelContactRequest
            // {
            //     FirstName = "Preston",
            //     LastName = "Weight",
            //     Email = "pweight+test1@nerdsunite.me",
            //     Tags = new List<string> { "Gala", "Hyper" }
            // };

            // var response = await client.AddUpdateContactAsync("9c5wHGZk7rn52UIHwicl", request);
            // Console.WriteLine(response.Id);
            // Console.WriteLine(string.Join(", ", response.Tags));

            // var apiClient = new HighLevelApiService("GO_HIGH_LEVEL_API_KEY", new LoggerFactory().CreateLogger<HighLevelApiService>());
            // var searchResponse = await apiClient.SearchContactAsync("pweight+test1@nerdsunite.me");

            // Console.WriteLine(searchResponse.FirstOrDefault()?.Id);
        
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static void AddingNumbersSequentially(int start, int limit)
    {
        // Add numberup from start to limit inclusive and sequentially
        // S = n/2(a + l)
        int total = limit / 2 * (start + limit);
        Console.WriteLine(total);
    }

    private static void StringNullStuff()
    {
        string myString = null;
        Console.WriteLine(string.IsNullOrWhiteSpace(myString));  // Outputs: True

        myString = "";
        Console.WriteLine(string.IsNullOrWhiteSpace(myString));  // Outputs: True

        myString = " ";
        Console.WriteLine(string.IsNullOrWhiteSpace(myString));  // Outputs: True

        myString = "Hello, World!";
        Console.WriteLine(string.IsNullOrWhiteSpace(myString));  // Outputs: False

        string myDefaultString = default!;
        Console.WriteLine(myDefaultString);
        Console.WriteLine(string.IsNullOrWhiteSpace(myDefaultString));  // Outputs: True
    }

    private static void OrderLineItemStuff()
    {
        var order = new Order
        {
            CouponDiscount = 1000,
            // Discounts = [new Discount { CouponDiscount = 1000, EligibleItems=["Win", "Switch"] }, new Discount { CouponDiscount = 1000, EligibleItems=["Win", "Galvan"]}],
            Discounts = [new Discount { CouponDiscount = 20, EligibleItems=["Win", "Switch"] }, new Discount { CouponDiscount = 10, EligibleItems=["Win", "Switch"]}],
            OrderItems =
            [
                new OrderItem { Name = "Win", LineTotal = 50, SingleItemPrice = 1, IsEligibleForDiscount = true, Quantity = 50},
                new OrderItem { Name = "Switch", LineTotal = 50, SingleItemPrice = 10, IsEligibleForDiscount = true, Quantity = 5},
                new OrderItem { Name = "Element", LineTotal = 50, SingleItemPrice = 10, IsEligibleForDiscount = false, Quantity = 5}
            ]
        };

        order.ApplyDiscount();

        // order.OrderItems.ForEach(item => Console.WriteLine($"LINE ITEM TOTALS: {item.Name} - {item.LineTotal}"));
        // Console.WriteLine($"ORDER TOTAL: {order.OrderItems.Sum(item => item.LineTotal)}");
    }

    private static void DateStuff()
    {
        string establishedOn = "04/04/2023";
        DateTimeOffset establishedOnDateTime = DateTimeOffset.ParseExact(establishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        // Console.WriteLine(establishedOnDateTime);
    }

    public static void StringStuff()
    {
        string accountNumber = "0917408123192837";
        string lastFour = accountNumber.Substring(accountNumber.Length - 4);
        string accountNumber2 = "2903718078401143";
        string lastFour2 = accountNumber2[^4..];

        Console.WriteLine(lastFour);
        Console.WriteLine(lastFour2);
    }
}

public class OrderItem
{
    public string Name { get; set; } = default!;
    public decimal LineTotal { get; set; }
    public decimal SingleItemPrice { get; set; }    
    public bool IsEligibleForDiscount { get; set; }
    public int Quantity { get; set; }
}

public class Discount
{
    public decimal CouponDiscount { get; set; }
    public List<string> EligibleItems { get; set; } = [];
}

public class Order
{
    public List<OrderItem> OrderItems { get; set; } = [];
    public List<Discount> Discounts { get; set; } = [];
    public decimal CouponDiscount { get; set; }

    public void ApplyDiscount()
    {
        // var eligibleItemsTotal = OrderItems.Where(item => item.IsEligibleForDiscount).Sum(item => item.SingleItemPrice);

        foreach (var discount in Discounts)
        {
            var eligibleItems = OrderItems.Where(item => discount.EligibleItems.Contains(item.Name));
            var eligibleItemsTotal = eligibleItems.Sum(item => item.SingleItemPrice);
            foreach (var item in eligibleItems)
            {
                item.LineTotal -= item.SingleItemPrice * item.Quantity / item.Quantity / eligibleItemsTotal * discount.CouponDiscount;
                // Console.WriteLine($"{item.Name} - {item.LineTotal}");
            }
        }

        // Original Sauce
        // foreach (var item in OrderItems)
        // {
        //     if (item.IsEligibleForDiscount)
        //     {
        //         item.LineTotal -= item.LineTotal / item.Quantity / eligibleItemsTotal * CouponDiscount;
        //     }
        // }
    }
}

