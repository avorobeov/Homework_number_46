using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Homework_number_46
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Queue<Client> clients = new Queue<Client>();

            clients.Enqueue(new Client(10));
            clients.Enqueue(new Client(50));
            clients.Enqueue(new Client(50));

            Supermarket supermarket = new Supermarket(clients);

            supermarket.Work();
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

        public bool TryPayPurchases(out Product product)
        {
            product = null;

            if (AmountPurchase > Money)
            {
                Console.WriteLine("К сожалению не хватает денег прийдётся что-то оставить в магазине!");

                product = ReturnProduct();

                return false;
            }

            return true;
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
        private Queue<Client> _clients;

        public Supermarket(Queue<Client> clients)
        {
            _clients = clients;

            FillShop();
        }

        public void Work()
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
                            isExit = TryServeСustomer(client);
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

        private bool TryServeСustomer(Client client)
        {
            while (client.TryPayPurchases(out Product product) == false)
            {
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
            CreateProducts(5, "молоко", 5);
            CreateProducts(5, "рыба", 10);
            CreateProducts(5, "мясо", 15);
        }

        private void CreateProducts(int quantity,string title,int price)
        {
            for (int i = 0; i < quantity; i++)
            {
                _products.Add(new Product(title, price));
            }
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
