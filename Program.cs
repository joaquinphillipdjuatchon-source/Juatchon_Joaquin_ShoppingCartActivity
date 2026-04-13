using System;

class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int Stock;

    public void Display()
    {
        Console.WriteLine($"{Id}. {Name} - ₱{Price} (Stock: {Stock})");
    }

    public bool HasStock(int qty)
    {
        return qty <= Stock;
    }

    public void Deduct(int qty)
    {
        Stock -= qty;
    }
}

class Program
{
    static void Main()
    {
        Product[] products = new Product[]
        {
            new Product { Id = 1, Name = "CPU", Price = 30000, Stock = 10 },
            new Product { Id = 2, Name = "Monitor", Price = 15000, Stock = 18 },
            new Product { Id = 3, Name = "Keyboard", Price = 650, Stock = 27 },
            new Product { Id = 4, Name = "Mouse", Price = 750, Stock = 39 },
            new Product { Id = 5, Name = "Webcam", Price = 400, Stock = 36 },
            new Product { Id = 6, Name = "Microphone", Price = 600, Stock = 19 },
            new Product { Id = 7, Name = "Printer", Price = 4500, Stock = 8 },
            new Product { Id = 8, Name = "Laptop", Price = 25000, Stock = 9 },
            new Product { Id = 9, Name = "Speakers", Price = 2500, Stock = 17 },
            new Product { Id = 10, Name = "Headphones", Price = 350, Stock = 17 }
        };

        int[] cartQty = new int[10];
        double[] cartTotal = new double[10];

        string input = "Y";

        while (input == "Y")
        {
            Console.WriteLine("\nCOMPUTER STORE MENU");
            for (int i = 0; i < products.Length; i++)
            {
                products[i].Display();
            }

            Console.WriteLine("Enter product number: ");
            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > products.Length)
            {
                Console.WriteLine("Invalid number.");
                continue;
            }
            Product selected = products[choice - 1];

            if (selected.Stock == 0)
            {
                Console.WriteLine("Sorry, this item is out of stock.");
                continue;
            }

            Console.Write("Enter quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
            {
                Console.WriteLine("Invalid quantity.");
                continue;
            }
            if (!selected.HasStock(qty))
            {
                Console.WriteLine("Sorry, we don't have enough stock available.");
                continue;
            }

            cartQty[choice - 1] += qty;
            cartTotal[choice - 1] = cartQty[choice - 1] * selected.Price;

            selected.Deduct(qty);

            Console.WriteLine("Item has been added to cart.");

            Console.Write("Do you want to add another item? (Y = Yes / N = No): ");
            input = Console.ReadLine();

            if (input != null)
                input = input.ToUpper();

            else
                input = "N";
        }
        double grandTotal = 0;

        Console.WriteLine("\nRECEIPT");
        for (int i = 0; i < products.Length; i++)
        {
            if (cartQty[i] > 0)
            {
                Console.WriteLine($"{products[i].Name} x{cartQty[i]} = ₱{cartTotal[i]}");
                grandTotal += cartTotal[i];
            }
        }
        Console.WriteLine($"Grand Total: ₱{grandTotal}");

        double discount = 0;
        if (grandTotal >= 5000)
        {
            discount = grandTotal * 0.20;
            Console.WriteLine($"Discount (20%): ₱{discount}");
        }
        double finalTotal = grandTotal - discount;
        Console.WriteLine($"Final Total: ₱{finalTotal}");

        Console.WriteLine("\nUPDATED STOCK");
        foreach (Product p in products)
        {
            p.Display();
        }
        Console.WriteLine("Thank you for shopping today!");
    }
}