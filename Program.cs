
namespace testing_app;

class Program
{
    static void Main(string[] args)
    {
        var order = new Order
        {
            CouponDiscount = 1000,
            Discounts = [new Discount { CouponDiscount = 1000, EligibleItems=["Win", "Switch"] }, new Discount { CouponDiscount = 1000, EligibleItems=["Win", "Galvan"]}],
            OrderItems =
            [
                new OrderItem { Name = "Win", LineTotal = 1000, SingleItemPrice = 500, IsEligibleForDiscount = true, Quantity = 2},
                new OrderItem { Name = "Switch", LineTotal = 2000, SingleItemPrice = 2000, IsEligibleForDiscount = true, Quantity = 1},
                new OrderItem { Name = "Galvan", LineTotal = 2000, SingleItemPrice = 2000, IsEligibleForDiscount = false, Quantity = 1}
            ]
        };

        order.ApplyDiscount();

        order.OrderItems.ForEach(item => Console.WriteLine($"LINE ITEM TOTALS: {item.Name} - {item.LineTotal}"));
        Console.WriteLine($"ORDER TOTAL: {order.OrderItems.Sum(item => item.LineTotal)}");
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
                Console.WriteLine($"{item.Name} - {item.LineTotal}");
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

