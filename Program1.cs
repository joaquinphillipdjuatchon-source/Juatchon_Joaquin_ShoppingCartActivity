using System;

class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int Stock;

    public void Display()
    {
        Console.WriteLine($"{Id}. {Name} - ₱{Price} (Stock: {Stock}) [{Category}]");
    }
    public string Category;

    public bool HasStock(int qty)
    {
        return qty <= Stock;
    }

    public void Deduct(int qty)
    {
        Stock -= qty;
    }

    public void Restock(int qty)
    {
        Stock += qty;
    }
}

class Program
{
    static void Main()
    {
        int receiptCounter = 1;
        string[] orderHistory = new string[100];
        int orderCount = 0;
        Product[] products = new Product[]
        {
            new Product { Id = 1, Name = "CPU", Price = 30000, Stock = 10, Category = "Electronics" },
            new Product { Id = 2, Name = "Monitor", Price = 15000, Stock = 18, Category = "Electronics" },
            new Product { Id = 3, Name = "Keyboard", Price = 650, Stock = 27, Category = "Electronics" },
            new Product { Id = 4, Name = "Mouse", Price = 750, Stock = 39, Category = "Electronics" },
            new Product { Id = 5, Name = "Laptop", Price = 25000, Stock = 36, Category = "Electronics" },
            new Product { Id = 6, Name = "Microphone", Price = 600, Stock = 19, Category = "Electronics" },
            new Product { Id = 7, Name = "Bottled Water", Price = 20, Stock = 8, Category = "Food" },
            new Product { Id = 8, Name = "Potato Chips", Price = 25, Stock = 9, Category = "Food" },
            new Product { Id = 9, Name = "Pants", Price = 400, Stock = 17, Category = "Clothing" },
            new Product { Id = 10, Name = "T-Shirt", Price = 350, Stock = 17, Category = "Clothing" }

        };

        int[] cartQty = new int[10];
        double[] cartTotal = new double[10];

        bool running = true;

        while (running)
        {
            Console.WriteLine("\nMY SHOPPING CART MENU");
            Console.WriteLine("1. Add Item");
            Console.WriteLine("2. View Cart");
            Console.WriteLine("3. Remove Item");
            Console.WriteLine("4. Update Quantity");
            Console.WriteLine("5. Clear Cart");
            Console.WriteLine("6. Checkout");
            Console.WriteLine("7. Search Product");
            Console.WriteLine("8. Filter by Category");
            Console.WriteLine("9. View Order History");
            Console.WriteLine("10. Exit");
            Console.Write("Choose option: ");

            int.TryParse(Console.ReadLine(), out int option);

            switch (option)
            {
                case 1:
                    Console.WriteLine("\nPRODUCT LIST");
                    for (int i = 0; i < products.Length; i++)
                        products[i].Display();

                    Console.Write("Enter product number: ");
                    if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > products.Length)
                    {
                        Console.WriteLine("Invalid choice.");
                        break;
                    }

                    Product selected = products[choice - 1];

                    if (selected.Stock == 0)
                    {
                        Console.WriteLine("Out of stock.");
                        break;
                    }

                    Console.Write("Enter quantity: ");
                    if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
                    {
                        Console.WriteLine("Invalid quantity.");
                        break;
                    }

                    if (!selected.HasStock(qty))
                    {
                        Console.WriteLine("Not enough stock.");
                        break;
                    }

                    Console.Write("Confirm add item? (Y/N): ");
                    string confirm = Console.ReadLine().ToUpper();

                    if (confirm == "Y")
                    {
                        cartQty[choice - 1] += qty;
                        cartTotal[choice - 1] = cartQty[choice - 1] * selected.Price;
                        selected.Deduct(qty);

                        Console.WriteLine("Item added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Item failed to add.");
                    }
                    break;

                case 2:
                    Console.WriteLine("\nYour Cart");
                    double tempTotal = 0;

                    for (int i = 0; i < products.Length; i++)
                    {
                        if (cartQty[i] > 0)
                        {
                            Console.WriteLine($"{products[i].Name} x{cartQty[i]} = ₱{cartTotal[i]}");
                            tempTotal += cartTotal[i];
                        }
                    }

                    Console.WriteLine($"Total: ₱{tempTotal}");
                    break;

                case 3:
                    Console.Write("Enter product number to remove: ");
                    int.TryParse(Console.ReadLine(), out int removeIndex);

                    if (removeIndex < 1 || removeIndex > products.Length || cartQty[removeIndex - 1] == 0)
                    {
                        Console.WriteLine("Item not in cart.");
                        break;
                    }

                    products[removeIndex - 1].Restock(cartQty[removeIndex - 1]);
                    cartQty[removeIndex - 1] = 0;
                    cartTotal[removeIndex - 1] = 0;

                    Console.WriteLine("Item removed from cart.");
                    break;

                case 4:
                    Console.Write("Enter product number: ");
                    int.TryParse(Console.ReadLine(), out int updateIndex);

                    if (updateIndex < 1 || updateIndex > products.Length || cartQty[updateIndex - 1] == 0)
                    {
                        Console.WriteLine("Item not in cart.");
                        break;
                    }

                    Console.Write("Enter new quantity: ");
                    int.TryParse(Console.ReadLine(), out int newQty);

                    Product item = products[updateIndex - 1];

                    item.Restock(cartQty[updateIndex - 1]);

                    if (!item.HasStock(newQty))
                    {
                        Console.WriteLine("Insufficient stock.");
                        item.Deduct(cartQty[updateIndex - 1]);
                        break;
                    }

                    cartQty[updateIndex - 1] = newQty;
                    cartTotal[updateIndex - 1] = newQty * item.Price;
                    item.Deduct(newQty);

                    Console.WriteLine("Quantity updated successfully.");
                    break;

                case 5:
                    for (int i = 0; i < products.Length; i++)
                    {
                        products[i].Restock(cartQty[i]);
                        cartQty[i] = 0;
                        cartTotal[i] = 0;
                    }
                    Console.WriteLine("Cart cleared successfully.");
                    break;

                case 6:
                    {
                        double grandTotal = 0;

                        Console.WriteLine($"\nReceipt: #{receiptCounter.ToString("D4")}");
                        Console.WriteLine($"Date: {DateTime.Now:MMMM dd, yyyy hh:mm tt}");
                        Console.WriteLine("\nRECEIPT");

                        for (int i = 0; i < products.Length; i++)
                        {
                            if (cartQty[i] > 0)
                            {
                                Console.WriteLine($"{products[i].Name} x{cartQty[i]} = ₱{cartTotal[i]}");
                                grandTotal += cartTotal[i];
                            }
                        }

                        if (grandTotal == 0)
                        {
                            Console.WriteLine("Your cart is empty. Nothing to checkout.");
                            break;
                        }

                        double discount = (grandTotal >= 5000) ? grandTotal * 0.20 : 0;
                        double finalTotal = grandTotal - discount;

                        Console.WriteLine($"\nGrand Total: ₱{grandTotal}");
                        Console.WriteLine($"Discount: ₱{discount}");
                        Console.WriteLine($"Final Total: ₱{finalTotal}");

                        double payment;

                        while (true)
                        {
                            Console.Write("Enter payment: ₱");

                            if (!double.TryParse(Console.ReadLine(), out payment))
                            {
                                Console.WriteLine("Invalid input. Enter numbers only.");
                                continue;
                            }

                            if (payment < finalTotal)
                            {
                                Console.WriteLine("Insufficient payment. Try again.");
                                continue;
                            }

                            break;
                        }

                        double change = payment - finalTotal;
                        Console.WriteLine($"Change: ₱{change}");

                        orderHistory[orderCount] =
                            $"Receipt #{receiptCounter.ToString("D4")} - Final Total: ₱{finalTotal}";
                        orderCount++;
                        receiptCounter++;

                        Console.WriteLine("\nCheckout complete! Returning to menu...");
                        Console.WriteLine("\nLOW STOCK ALERT:");

                        bool lowStockFound = false;

                        for (int i = 0; i < products.Length; i++)
                        {
                            if (products[i].Stock > 0 && products[i].Stock <= 5)
                            {
                                Console.WriteLine($"{products[i].Name} has only {products[i].Stock} stock(s) left.");
                                lowStockFound = true;
                            }
                        }

                        if (!lowStockFound)
                        {
                            Console.WriteLine("No low stock items.");
                        }
                        break;
                    }

                case 7:
                    {
                        Console.Write("Enter product name to search: ");
                        string keyword = Console.ReadLine().ToLower();

                        bool found = false;

                        Console.WriteLine("\nSearch Results:");

                        for (int i = 0; i < products.Length; i++)
                        {
                            if (products[i].Name.ToLower().Contains(keyword))
                            {
                                products[i].Display();
                                found = true;
                            }
                        }

                        if (!found)
                        {
                            Console.WriteLine("No matching product found.");
                        }

                        break;
                    }

                case 8:
                    Console.WriteLine("\nSelect Category");
                    Console.WriteLine("1. Electronics");
                    Console.WriteLine("2. Food");
                    Console.WriteLine("3. Clothing");
                    Console.Write("Enter Choice: ");
                    int.TryParse(Console.ReadLine(), out int catChoice);

                    string selectedCategory = "";

                    switch (catChoice)
                    {
                        case 1: selectedCategory = "Electronics"; break;
                        case 2: selectedCategory = "Food"; break;
                        case 3: selectedCategory = "Clothing"; break;
                        default:
                            Console.WriteLine("Invalid category.");
                            break;
                    }

                    if (selectedCategory != "")
                    {
                        Console.WriteLine($"\nProducts in {selectedCategory.ToUpper()}");

                        bool found = false;

                        for (int i = 0; i < products.Length; i++)
                        {
                            if (products[i].Category == selectedCategory)
                            {
                                products[i].Display();
                                found = true;
                            }
                        }

                        if (!found)
                        {
                            Console.WriteLine("No products in this category.");
                        }
                    }
                    break;

                case 9:
                    Console.WriteLine("\nOrder History");

                    if (orderCount == 0)
                    {
                        Console.WriteLine("No orders yet.");
                    }
                    else
                    {
                        for (int i = 0; i < orderCount; i++)
                        {
                            Console.WriteLine(orderHistory[i]);
                        }
                    }
                    break;

                case 10:
                    running = false;
                    Console.WriteLine("Thank you for shopping today!");
                    break;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}