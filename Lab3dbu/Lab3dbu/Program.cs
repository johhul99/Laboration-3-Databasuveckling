using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using MongoDB.Driver;
using MongoDB.Bson;
using Lab3dbu;
using System.Xml.Linq;
using System.Text.Json;

Connection con = new Connection();
var settings = MongoClientSettings.FromConnectionString(con.ConnectionUri());
settings.ServerApi = new ServerApi(ServerApiVersion.V1);
var client = new MongoClient(settings);
var database = client.GetDatabase("lab3dbu");
var dbproducts = database.GetCollection<BsonDocument>("products");

var filter = Builders<BsonDocument>.Filter.Empty;
var documents = await dbproducts.Find(filter).ToListAsync();
List<Product> products = new List<Product>();

await LoadProductsAsync();

CustomerGold Test1 = new CustomerGold("Test1", "Test1", new List<Product>());
CustomerSilver Test2 = new CustomerSilver("Test2", "Test2", new List<Product>());
CustomerBronze Test3 = new CustomerBronze("Test3", "Test3", new List<Product>());
Customer Bas1 = new Customer("Bas1", "Bas1", new List<Product>());


List<Customer> Customers = new List<Customer> { { Test1 }, { Test2 }, { Test3 }, { Bas1 } }; //För att kunna logga in.



string filePath = "data.json";

LoadObjects();

while (1 == 1) // Så att man kan gå tillbaka till startskärm 
{
    Console.Clear();
    Console.WriteLine("Välkommen till Johans Kiosk!");
    Console.WriteLine("1. Logga in");
    Console.WriteLine("2. Registrera ny kund");
    Console.WriteLine("Valfri annan symbol för att avsluta.");
    Console.WriteLine("Vänligen svara med 1 eller 2 för att gå vidare.");
    ConsoleKeyInfo answer1 = Console.ReadKey();
    if (answer1.Key == ConsoleKey.D1)
    {
        Console.Clear();
        Console.WriteLine("Ange användarnamn:");
        string UsernameInput = Console.ReadLine();
        bool UsernameSuccess = LoginUsername(UsernameInput);
        if (UsernameSuccess) //Kontrollerar först användarnamn för att kunna ge möjlighet för att registrera om användarnamnet ej finns eller skriva in lösenord igen ifall man skriver fel längre ner
        {
            while (1 == 1)
            {
                Console.WriteLine("Ange lösenord.");
                string PasswordInput = Console.ReadLine();

                bool LoginSuccess = Login(UsernameInput, PasswordInput);
                if (LoginSuccess)
                {


                    foreach (Customer C in Customers)
                    {
                        if (C.Username == UsernameInput)
                        {
                            Customer Active = C; //Ger inloggad kund en tillfällig variabel för att kunna kalla på metoder och ändra i kundvagn
                            string valuta = "SEK"; //Tillåter inloggad användare att välja valuta som återställs till SEK varje gång man loggar ut

                            while (1 == 1) //Så att man kan komma tillbaka till menyn nedan
                            {
                                Console.Clear();
                                Console.WriteLine("Välkommen " + UsernameInput + "!");
                                Console.WriteLine("Meny");
                                Console.WriteLine("1. Handla");
                                Console.WriteLine("2. Kundvagn");
                                Console.WriteLine("3. Ändra valuta");
                                Console.WriteLine("4. Lägg till produkt");
                                Console.WriteLine("5. Ta bort produkt");
                                Console.WriteLine("6. Logga ut");
                                ConsoleKeyInfo answer = Console.ReadKey();

                                if (answer.Key == ConsoleKey.D1)
                                {
                                    while (1 == 1)
                                    {
                                        Console.Clear();
                                        Product.ShowProduct(valuta, products);
                                        Console.WriteLine();
                                        Console.WriteLine("Skriv in siffran framför den produkt du vill lägga till i varukorgen och tryck enter eller valfri symbol och/eller enter för att gå tillbaka till menyn.");
                                        string answer2 = Console.ReadLine(); //ReadLine så att man inte råkar lägga till något man ej vill ha i varukorgen
                                        if (answer2 == "1" || answer2 == "2" || answer2 == "3")
                                        {
                                            for (int i = 0; i < products.Count; i++)
                                            {

                                                if (Convert.ToInt32(answer2) == i + 1)
                                                {
                                                    Console.Clear();
                                                    Active.AddToCart(products[i]);
                                                    Console.ReadKey();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Console.Clear();
                                            Console.WriteLine("Du blir omdirigerad tillbaka til menyn.");
                                            Console.ReadKey();
                                            break;
                                        }
                                    }
                                }
                                else if (answer.Key == ConsoleKey.D2)
                                {
                                    Console.Clear();
                                    Active.ShowCart(valuta);
                                    Console.WriteLine("Skriv in 1 för att avsluta köp eller valfri symbol för att gå tillbaka.");
                                    string answer2 = Console.ReadLine();
                                    if (answer2 == "1")
                                    {
                                        Console.Clear();
                                        Active.CheckoutCart(valuta);
                                        Console.WriteLine("Du återvänder nu till menyn.");
                                        Console.ReadKey();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Du återvänder nu till menyn.");
                                        Console.ReadKey();
                                    }

                                }
                                else if (answer.Key == ConsoleKey.D3)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Nuvarande valuta: " + valuta);
                                    Console.WriteLine("Vilken valuta vill du se priser i?");
                                    Console.WriteLine("SEK");
                                    Console.WriteLine("EUR");
                                    Console.WriteLine("GBP");
                                    Console.WriteLine("Skriv in önskad valuta och klicka enter eller annat svar för att gå tillbaka till menyn.");
                                    string answer2 = Console.ReadLine();
                                    if (answer2 == valuta)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Du ser redan priser i " + answer2 + ".");
                                        Console.ReadKey();
                                    }
                                    else if (answer2 == "GBP")
                                    {
                                        Console.Clear();
                                        valuta = "GBP";
                                        Console.WriteLine("Du har bytt valuta till GBP.");
                                        Console.ReadKey();
                                    }
                                    else if (answer2 == "EUR")
                                    {
                                        Console.Clear();
                                        valuta = "EUR";
                                        Console.WriteLine("Du har bytt valuta till EUR.");
                                        Console.ReadKey();
                                    }
                                    else if (answer2 == "SEK")
                                    {
                                        Console.Clear();
                                        valuta = "SEK";
                                        Console.WriteLine("Du har bytt valuta till SEK.");
                                        Console.ReadKey();
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Du blir omdirigerad till menyn.");
                                        Console.ReadKey();
                                    }
                                }
                                else if (answer.Key == ConsoleKey.D4)
                                {
                                    while (1 == 1)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Lägg till produkt:");
                                        Console.WriteLine("Namn:");
                                        string name = Console.ReadLine();
                                        Console.WriteLine("Pris i kronor:");
                                        try
                                        {
                                            double price = Convert.ToDouble(Console.ReadLine());
                                            Product newProduct = new Product(name, price);
                                            await AddProductAsync(newProduct);
                                            Console.WriteLine("Du blir omdirigerad till menyn.");
                                            Console.ReadKey();
                                            break;

                                        }
                                        catch
                                        {
                                            Console.Clear();
                                            Console.WriteLine("Vänligen skriv in ett tal, om decimal önskas använd: ,");
                                        }
                                    }
                                }
                                else if (answer.Key == ConsoleKey.D5)
                                {
                                    Console.Clear();
                                    Product.ShowProduct(valuta, products);
                                    Console.WriteLine();
                                    Console.WriteLine("Skriv in siffran framför den produkt du vill ta bort ur sortimentet och tryck enter eller valfri symbol och/eller enter för att gå tillbaka till menyn.");
                                    try
                                    {
                                        int i = Convert.ToInt32(Console.ReadLine());
                                        string nameOfProduct = products[i-1].Name;
                                        Console.Clear();
                                        await RemoveProductAsync(products[i-1]);
                                        Console.WriteLine("Du blir omdirigerad till menyn!");
                                        Console.ReadKey();
                                    }
                                    catch
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Du blir omdirigerad till menyn!");
                                        Console.ReadKey();
                                    }

                                }
                                else if (answer.Key == ConsoleKey.D6)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Hejdå.");
                                    Console.ReadKey();
                                    break;
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("Vänligen svara med 1, 2, 3, 4, 5 eller 6.");
                                    Console.ReadKey();
                                }


                            }
                        }
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("Fel lösenord, vill du försöka igen? Skriv JA. Skriv annars valfri symbol eller klicka bara enter för att komma till hemskärmen.");
                    string answer = Console.ReadLine();
                    if (answer == "JA")
                    {
                        Console.Clear();
                        Console.WriteLine("Ange användarnamn:");
                        Console.WriteLine(UsernameInput);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("Användarnamnet finns ej registrerat, vill du registrera dig? Skriv: JA. Skriv annars valfri symbol eller klicka enter för att komma till hemskärmen.");
            string answer = Console.ReadLine();
            if (answer == "JA")
            {
                Console.Clear();
                Console.WriteLine("Ange användarnamn:");
                Console.WriteLine(UsernameInput);
                Console.WriteLine("Skriv in ditt önskade lösenord.");
                string InputPass = Console.ReadLine();
                Customers.Add(CustomerType(UsernameInput, InputPass));
                Console.WriteLine("Du är nu registrerad och kan logga in, du blir omdirigerad till hemskärmen.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Du blir omdirigerad till hemskärmen.");
            }
        }

    }
    else if (answer1.Key == ConsoleKey.D2)
    {
        while (1 == 1)
        {
            Console.Clear();
            Console.WriteLine("Skriv in önskat användarnamn.");
            string UsernameInput = Console.ReadLine();

            bool RegistrationSuccess = Register(UsernameInput);
            if (RegistrationSuccess)
            {
                Console.WriteLine("Skriv in önskat lösenord.");
                string PasswordInput = Console.ReadLine();
                Customers.Add(CustomerType(UsernameInput, PasswordInput));
                Console.WriteLine("Du är nu registrerad, du kan nu logga in och dirigeras om till hemskärmen.");
                Console.ReadKey();
                break;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Användarnamn upptaget! Skriv JA för att försöka igen eller valfri annan symbol eller klicka enter för att komma till hemskärmen.");
                string answer = Console.ReadLine();
                if (answer == "JA")
                {
                }
                else
                {
                    break;
                }
            }
        }
    }
    else
    {
        Console.WriteLine("Hejdå!");
        Console.ReadKey();
        break;
    }
}
SaveObjects();



bool Register(string user)
{
    foreach (Customer c in Customers)
    {
        if (c.Username == user) //Så två användare ej kan ha samma användarnamn
        {
            return false;
        }
    }
    return true;
}

Customer CustomerType(string UsernameInput, string InputPass) //Så man kan välja kundtyp vid registrering (i syfte av uppgiften, ologiskt sätt att bygga på annars som vi pratat om :))
{
    Customer c2;
    while (1 == 1)
    {
        Console.WriteLine("Skriv in kundtyp, GOLD, SILVER, BRONZE eller BAS.");
        string kundtyp = Console.ReadLine();
        if (kundtyp == "GOLD")
        {
            c2 = new CustomerGold(UsernameInput, InputPass, new List<Product>());
            break;
        }
        else if (kundtyp == "SILVER")
        {
            c2 = new CustomerSilver(UsernameInput, InputPass, new List<Product>());
            break;
        }
        else if (kundtyp == "BRONZE")
        {
            c2 = new CustomerBronze(UsernameInput, InputPass, new List<Product>());
            break;
        }
        else if (kundtyp == "BAS")
        {
            c2 = new Customer(UsernameInput, InputPass, new List<Product>());
            break;
        }
        else
        {
            Console.WriteLine("Vänligen svara med ett av alternativen.");
        }
    }
    return c2;
}

bool LoginUsername(string user)
{
    foreach (Customer c in Customers)
    {
        if (c.Username == user)
        {
            return true;
        }
    }
    return false;
}

bool Login(string user, string pass)
{
    foreach (Customer c in Customers)
    {
        if (c.Username == user && c.Password == pass)
        {
            return true;
        }
    }
    return false;
}

void SaveObjects()
{
    var options = new JsonSerializerOptions
    {
        WriteIndented = true,
        Converters = { new CustomerConverter() }
    };

    string jsonString = JsonSerializer.Serialize(Customers, options);
    File.WriteAllText(filePath, jsonString);
}

void LoadObjects()
{
    var options = new JsonSerializerOptions
    {
        Converters = { new CustomerConverter() }
    };

    try //Ifall filen ej finns kan programmet ändå köras och utgå ifrån endast listan, första gången programmet körs skapas filen i SaveObjects ovan
    {
        string savedJson = File.ReadAllText(filePath);
        Customers = JsonSerializer.Deserialize<List<Customer>>(savedJson, options);
    }
    catch (Exception ex) //Finns filen så händer inget istället för att programmet ska krascha
    {
    }
}

async Task AddProductAsync(Product p)
{    
    var pDocument = new BsonDocument
    {
        {"name", p.Name },
        {"price", p.Price }
    };
    await dbproducts.InsertOneAsync(pDocument);
    Console.WriteLine($"{p.Name} är nu tillagd i sortimentet!");
    products.Add(p);
}

async Task RemoveProductAsync(Product p)
{
    var filter = Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("name", p.Name), Builders<BsonDocument>.Filter.Eq("price", p.Price));
    var result = await dbproducts.DeleteOneAsync(filter);
    Console.WriteLine($"{p.Name} är nu borttagen ur sortimentet");
    products.Remove(p);
}

async Task LoadProductsAsync()
{
    products = new List<Product>();
    foreach (var document in documents)
    {
        string name = document.GetValue("name").AsString;
        double price = document.GetValue("price").AsDouble;

        products.Add(new Product(name, price));
    }
}



