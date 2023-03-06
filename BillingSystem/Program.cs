using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Define pizza sizes and their prices
        var pizzaSizes = new Dictionary<string, int>()
        {
            { "Small", 1200 },
            { "Medium", 1400 },
            { "Large", 1600 }
        };

        // Define basic and deluxe toppings and their prices by size
        var toppings = new Dictionary<string, Dictionary<string, int>>()
        {
            {
                "Basic", new Dictionary<string, int>()
                {
                    { "Small", 50 },
                    { "Medium", 75 },
                    { "Large", 100 }
                }
            },
            {
                "Deluxe", new Dictionary<string, int>()
                {
                    { "Small", 200 },
                    { "Medium", 300 },
                    { "Large", 400 }
                }
            }
        };

        Console.WriteLine("Enter your order in the following format: Size – Topping, Topping, Topping …");

        // Parse the user input
        var orders = Console.ReadLine()
            .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(order => order.Trim())
            .ToList();

        // Calculate the total price and toppings count for each pizza size
        var totals = new Dictionary<string, Tuple<int, double>>();
        foreach (var order in orders)
        {
            var parts = order.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(part => part.Trim())
                .ToList();

            var size = parts[0];
            var toppingsList = parts.Skip(1).Select(topping => topping.Trim()).ToList();

            if (!pizzaSizes.ContainsKey(size))
            {
                Console.WriteLine($"Invalid size: {size}");
                continue;
            }

            var price = pizzaSizes[size];

            var basicToppingsCount = toppingsList
                .Where(topping => toppings["Basic"].ContainsKey(size) && toppings["Basic"][size] > 0)
                .Count();
            price += basicToppingsCount * toppings["Basic"][size];

            var deluxeToppingsCount = toppingsList
                .Where(topping => toppings["Deluxe"].ContainsKey(size) && toppings["Deluxe"][size] > 0)
                .Count();
            price += deluxeToppingsCount * toppings["Deluxe"][size];

            var totalPrice = price + price * 0.16; // add VAT
            totalPrice = Math.Round(totalPrice, 2, MidpointRounding.AwayFromZero);

            if (!totals.ContainsKey(size))
            {
                totals[size] = Tuple.Create(0, 0.0);
            }
            var count = totals[size].Item1 + 1;
            var subtotal = totals[size].Item2 + totalPrice;
            totals[size] = Tuple.Create(count, subtotal);

            Console.WriteLine($"{count} {size}, {basicToppingsCount + deluxeToppingsCount} Topping Pizza - {string.Join(", ", toppingsList)}: {totalPrice:F2}");
        }

        // Calculate the subtotal, VAT and total for all orders
        var subtotalAll = totals.Values.Sum(total => total.Item2);
        var vat = Math.Ceiling(subtotalAll * 0.16);
        var totalAll = subtotalAll + vat;

        Console.WriteLine($"SUB-TOTAL: {subtotalAll:F2} VAT: {vat:F2} TOTAL: {totalAll:F2}");

        Console.ReadKey();
    }
}
