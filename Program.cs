
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
static void SerializeProductList(List<Product> productList)
{
    XmlSerializer serializer = new XmlSerializer(typeof(List<Product>));
    using (TextWriter writer = new StreamWriter("product.xml"))
    {
        serializer.Serialize(writer, productList);
    }
    Console.WriteLine("Xml файл сериализован");
}

static List<Product> DeserializeProductList()
{
    List<Product> productList;
    XmlSerializer serializer = new XmlSerializer(typeof(List<Product>));
    using (TextReader reader = new StreamReader("product.xml"))
    {
        productList = (List<Product>)serializer.Deserialize(reader);
    }
    Console.WriteLine("Xml файл десериализован");
    return productList;
}

Menu();

static void ListProducts()
{
    List<Product> products = new List<Product>();

    XmlDocument xDoc = new XmlDocument();
    xDoc.Load("product.xml");

    XmlElement xRoot = xDoc.DocumentElement;

    if (xRoot != null)
    {
        foreach (XmlElement xnode in xRoot)
        {
            Product product = new Product();

            XmlNode attr = xnode.Attributes.GetNamedItem("name");
            product.Name = attr?.Value;

            foreach (XmlNode childnode in xnode.ChildNodes)
            {
                if (childnode.Name == "price")
                {
                    product.Price = float.Parse(childnode.InnerText);
                }
                if (childnode.Name == "quantity")
                {
                    product.Quantity = float.Parse(childnode.InnerText);
                }
            }

            products.Add(product);
        }

        foreach (var item in products)
        {
            Console.WriteLine($"Name = {item.Name} \t Price = {item.Price} \t Quantity = {item.Quantity}");
        }
    }

    Console.WriteLine("Сделайте выбор для продолжения...");
    Console.ReadKey();
}

static void AddProduct()
{
    XmlDocument xDoc = new XmlDocument();
    xDoc.Load("product.xml");

    XmlElement xRoot = xDoc.DocumentElement;

    XmlElement foodElement = xDoc.CreateElement("food");

    XmlAttribute nameAttr = xDoc.CreateAttribute("name");
    XmlElement priceElement = xDoc.CreateElement("price");
    XmlElement quantityElement = xDoc.CreateElement("quantity");

    Console.WriteLine("Введите название продукта:");
    string name = Console.ReadLine();
    nameAttr.Value = name;

    Console.WriteLine("Введите цену продукта:");
    float price = float.Parse(Console.ReadLine());
    priceElement.InnerText = price.ToString();

    Console.WriteLine("Введите количество продукта:");
    float quantity = float.Parse(Console.ReadLine());
    quantityElement.InnerText = quantity.ToString();

    foodElement.Attributes.Append(nameAttr);

    foodElement.AppendChild(priceElement);
    foodElement.AppendChild(quantityElement);

    xRoot.AppendChild(foodElement);
    xDoc.Save("product.xml");

    Console.WriteLine("Продукт добавлен успешно.");
    Console.WriteLine("Сделайте выбор для продолжения...");
    Console.ReadKey();
}

static void Menu()
{
    bool exit = false;
    while (!exit)
    {
        Console.Clear();
        Console.WriteLine("Выберите опцию:");
        Console.WriteLine("1. Просмотреть продукты.");
        Console.WriteLine("2. Добавить продукт.");
        Console.WriteLine("3. Выход из программы.");

        string option = Console.ReadLine();
        switch (option)
        {
            case "1":
                ListProducts();
                break;
            case "2":
                AddProduct();
                break;
            case "3":
                exit = true;
                break;
            default:
                Console.WriteLine("Некорректный ввод. Попробуйте еще раз.");
                break;
        }
    }
}


public class Product
{
    public string Name { get; set; }
    public float Price { get; set; }
    public float Quantity { get; set; }
}


