using System;
using System.Collections.Generic;

namespace Homework_number_46
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Supermarket supermarket = new Supermarket();

            supermarket.StartWork();
        }
    }

    class Product
    {
        public Product(string title, int price)
        {
            Title = title;
            Price = price;
        }

        public string Title { get; private set; }
        public int Price { get; private set; }
    }

    class Client
    {
        private List<Product> _products = new List<Product>();

        public Client(int money)
        {
            Money = money;
        }

        public int Money { get; private set; }
        public int AmountPurchase => GetAmountPurchases();

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public Product ReturnProduct()
        {
            Random random = new Random();

            Product product = null;

            if (_products.Count > 0)
            {
                product = _products[random.Next(_products.Count)];

                _products.Remove(product);

                Console.WriteLine($"К сожалению пришлось вернуть: {product.Title}");
            }

            return product;
        }

        private int GetAmountPurchases()
        {
            int amountPurchase = 0;

            for (int i = 0; i < _products.Count; i++)
            {
                amountPurchase += _products[i].Price;
            }

            return amountPurchase;
        }
    }

    class Supermarket
    {
        private List<Product> _products = new List<Product>();
        private Queue<Client> _clients = new Queue<Client>();

        public Supermarket()
        {
            FillShop();
        }

        public void StartWork()
        {
            const string CommandExit = "Exit";
            const string CommandPayPurchases = "Pay";

            while (_clients.Count > 0)
            {
                Client client = _clients.Dequeue();

                bool isExit = false;
                string userInput;

                while (isExit == false)
                {
                    ShowProductList();

                    Console.Write("\nДля получения товара ведите название товара\n" +
                                  $"Для оплаты покупок ведите команду: {CommandPayPurchases}\n" +
                                  $"Для завершения покупок ведите команду: {CommandExit}\n");
                    userInput = Console.ReadLine();

                    switch (userInput)
                    {
                        case CommandExit:
                            isExit = true;
                            break;

                        case CommandPayPurchases:
                            isExit = TryPayPurchases(client);
                            break;

                        default:
                            TryAddProduct(userInput, client);
                            break;
                    }

                    Console.WriteLine("\nДля продолжения ведите любую клавишу...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        private bool TryPayPurchases(Client client)
        {
            while (client.AmountPurchase > client.Money)
            {
                Console.WriteLine("К сожалению вам не хватает денег прийдётся что-то оставить в магазине!");

                Product product = client.ReturnProduct();

                if (product != null)
                {
                    _products.Add(product);
                }
            }

            return true;
        }

        private void ShowProductList()
        {
            Console.WriteLine($"\n\nСписок товаров\n");

            for (int i = 0; i < _products.Count; i++)
            {
                Console.WriteLine($"{_products[i].Title}-{_products[i].Price}");
            }
        }

        private void FillShop()
        {
            _products.Add(new Product("рыба", 10));
            _products.Add(new Product("рыба", 10));
            _products.Add(new Product("рыба", 10));
            _products.Add(new Product("рыба", 10));
            _products.Add(new Product("рыба", 10));
            _products.Add(new Product("рыба", 10));
            _products.Add(new Product("мясо", 15));
            _products.Add(new Product("мясо", 15));
            _products.Add(new Product("мясо", 15));
            _products.Add(new Product("мясо", 15));
            _products.Add(new Product("мясо", 15));
            _products.Add(new Product("мясо", 15));
            _products.Add(new Product("молоко", 5));
            _products.Add(new Product("молоко", 5));
            _products.Add(new Product("молоко", 5));
            _products.Add(new Product("молоко", 5));
            _products.Add(new Product("молоко", 5));
            _products.Add(new Product("молоко", 5));

            _clients.Enqueue(new Client(10));
            _clients.Enqueue(new Client(50));
            _clients.Enqueue(new Client(50));
        }

        private void TryAddProduct(string userInput, Client client)
        {
            Product product = null;

            for (int i = 0; i < _products.Count; i++)
            {
                if (_products[i].Title == userInput)
                {
                    product = _products[i];
                    _products.Remove(product);

                    break;
                }
            }

            if (product != null)
            {
                client.AddProduct(product);

                Console.WriteLine($"\n\nВы взяли {product.Title} его цена {product.Price}");
            }
            else
            {
                Console.WriteLine("\n\nК сожалению такого продукта нет в наличии!");
            }
        }
    }
}
