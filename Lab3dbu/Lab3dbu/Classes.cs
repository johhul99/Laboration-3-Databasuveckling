using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab3dbu
{
    public class Product //Produktklass
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public Product(string Name, double Price)
        {
            this.Name = Name;
            this.Price = Price;
        }

        public static void ShowProduct(string valuta, List<Product> products) //Så att man ej behöver ändra i konsollapplikationen om man lägger till produkter i kioskens utbud utan bara lägger till i products som ligger i program.cs
        {
            double valutaConversion = 1;

            if (valuta == "EUR")
            {
                valutaConversion = 0.089;
            }
            else if (valuta == "GBP")
            {
                valutaConversion = 0.074;
            }

            for (int i = 0; i < products.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + products[i].Name + ": " + (products[i].Price * valutaConversion) + " " + valuta + " St");
            }

        }

    }

    public class Customer
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public List<Product> Cart { get; set; }

        public Customer(string Username, string Password, List<Product> Cart) //Så att Customer serialieseras korrekt från Json, annars kunde man skippat Cart i konstruktorn
        {
            this.Username = Username;
            this.Password = Password;
            this.Cart = Cart ?? new List<Product>();
        }

        public List<Product> AddToCart(Product p)
        {
            Cart.Add(p);
            Console.WriteLine("1st " + p.Name + " har blivit tillagd i din varukorg.");
            return Cart;
        }

        public virtual double ShowCart(string valuta) //Double för att kunna kalla på basmetoden i subkklasser
        {
            double ColaCounter = 0;
            double PigelinCounter = 0;
            double MarabouCounter = 0;
            double valutaConversion = 1;
            double PigelinPrice = 0;
            double ColaPrice = 0;
            double MarabouPrice = 0;


            if (valuta == "EUR")
            {
                valutaConversion = 0.089;
            }
            else if (valuta == "GBP")
            {
                valutaConversion = 0.074;
            }

            foreach (Product p in Cart)
            {
                if (p.Name == "Pigelin")
                {
                    PigelinCounter++;
                    PigelinPrice = p.Price;
                }
                else if (p.Name == "Coca Cola")
                {
                    ColaCounter++;
                    ColaPrice = p.Price;
                }
                else if (p.Name == "Marabou")
                {
                    MarabouCounter++;
                    MarabouPrice = p.Price;
                }

            }

            double PigelinPriceTotal = PigelinPrice * PigelinCounter;
            double ColaPriceTotal = ColaPrice * ColaCounter;
            double MarabouPriceTotal = MarabouPrice * MarabouCounter;

            if (PigelinCounter > 0)
            {
                Console.WriteLine("Pigelin " + PigelinPrice + "kr st x" + PigelinCounter + " Totalt:" + (PigelinPriceTotal * valutaConversion) + " " + valuta);
            }
            if (ColaCounter > 0)
            {
                Console.WriteLine("Coca Cola " + ColaPrice + "kr st x" + ColaCounter + " Totalt:" + (ColaPriceTotal * valutaConversion) + " " + valuta);
            }
            if (MarabouCounter > 0)
            {
                Console.WriteLine("Marabou " + MarabouPrice + "kr st x" + MarabouCounter + " Totalt:" + (MarabouPriceTotal * valutaConversion) + " " + valuta);
            }
            Console.WriteLine("Totalt: " + ((MarabouPriceTotal + PigelinPriceTotal + ColaPriceTotal) * valutaConversion) + " " + valuta);

            double total = ((MarabouPriceTotal + PigelinPriceTotal + ColaPriceTotal) * valutaConversion);
            return total;
        }

        public double ToString(string valuta) //double för att kunna kalla på metod i CheckoutCart
        {
            Console.WriteLine("Username: " + Username + " Password: " + Password);
            double total = ShowCart(valuta);
            return total;
        }

        public virtual List<Product> CheckoutCart(string valuta)
        {
            double total = ToString(valuta);
            Console.WriteLine("Din slutsumma är " + total + " " + valuta);
            Cart = new List<Product>();
            return Cart;
        }
    }

    [JsonConverter(typeof(CustomerConverter))]
    public class CustomerGold : Customer
    {
        public CustomerGold(string Username, string Password, List<Product> Cart) : base(Username, Password, Cart)
        {
        }
        public override double ShowCart(string valuta)
        {
            double total = base.ShowCart(valuta);
            Console.WriteLine("Med din rabatt på 15%: " + (total * 0.85) + " " + valuta);//För att ge rätt rabatt
            return total;

        }

        public override List<Product> CheckoutCart(string valuta)
        {
            double total = ToString(valuta);
            Console.WriteLine("Din slutsumma är " + (total * 0.85) + " " + valuta);//För att ge rätt rabatt
            Cart = new List<Product>();//"Tömmer" kundvagnen
            return Cart;
        }

    }

    [JsonConverter(typeof(CustomerConverter))]
    public class CustomerSilver : Customer //Ser likadan ut som CustomerGold med en annan rabatt given. Samma gäller CustomerBronze nedan
    {
        public CustomerSilver(string Username, string Password, List<Product> Cart) : base(Username, Password, Cart)
        {
        }
        public override double ShowCart(string valuta)
        {
            double total = base.ShowCart(valuta);
            Console.WriteLine("Med din rabatt på 10%: " + (total * 0.90) + " " + valuta);
            return total;
        }

        public override List<Product> CheckoutCart(string valuta)
        {
            double total = ToString(valuta);
            Console.WriteLine("Din slutsumma är " + (total * 0.9) + " " + valuta);
            Cart = new List<Product>();
            return Cart;
        }
    }

    [JsonConverter(typeof(CustomerConverter))]
    public class CustomerBronze : Customer
    {
        public CustomerBronze(string Username, string Password, List<Product> Cart) : base(Username, Password, Cart)
        {
        }
        public override double ShowCart(string valuta)
        {
            double total = base.ShowCart(valuta);
            Console.WriteLine("Med din rabatt på 5%: " + (total * 0.95) + " " + valuta);
            return total;
        }

        public override List<Product> CheckoutCart(string valuta)
        {
            double total = ToString(valuta);
            Console.WriteLine("Din slutsumma är " + (total * 0.95) + " " + valuta);
            Cart = new List<Product>();
            return Cart;
        }

    }

    public class CustomerConverter : JsonConverter<Customer> //För att serialiseringen ska ske korrekt
    {
        public override Customer Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                var root = doc.RootElement;
                string customerType = root.GetProperty("CustomerType").GetString();
                string username = root.GetProperty("Username").GetString();
                string password = root.GetProperty("Password").GetString();

                List<Product> cart = JsonSerializer.Deserialize<List<Product>>(root.GetProperty("Cart").GetRawText(), options);

                return customerType switch
                {
                    "CustomerGold" => new CustomerGold(username, password, cart),
                    "CustomerSilver" => new CustomerSilver(username, password, cart),
                    "CustomerBronze" => new CustomerBronze(username, password, cart),
                    _ => new Customer(username, password, cart),
                };
            }
        }

        public override void Write(Utf8JsonWriter writer, Customer value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Username", value.Username);
            writer.WriteString("Password", value.Password);
            writer.WriteString("CustomerType", value.GetType().Name);
            writer.WritePropertyName("Cart");
            JsonSerializer.Serialize(writer, value.Cart, options);
            writer.WriteEndObject();
        }
    }
}
