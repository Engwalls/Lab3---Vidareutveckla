using System;
using System.Collections.Generic;

// Coffee Machine - Erik Engvall NET22

namespace HotDrinkStation
{
    // Interface for representing a warm drink
    public interface IWarmDrink
    {
        void Consume();
    }

    // Class for implementing hot water
    internal class Water : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Warm water is served.");
        }
    }

    // Class for implementing coffee
    internal class Coffee : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Coffee is served.");
        }
    }

    // Class for implementing cappuccino
    internal class Cappuccino : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Cappuccino is served.");
        }
    }

    // Class for implementing hot chocolate
    internal class HotChocolate : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Hot chocolate is served.");
        }
    }

    // Interface for creating warm drinks
    public interface IWarmDrinkFactory
    {
        IWarmDrink Prepare(int total);
    }

    // Class for creating hot water
    internal class HotWaterFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pour {total} ml hot water in your cup");
            return new Water();
        }
    }

    // Class for creating coffee
    internal class CoffeeFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Grind coffee beans and brew {total} ml coffee");
            return new Coffee();
        }
    }

    // Class for creating cappuccino
    internal class CappuccinoFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Grind coffee beans, brew {total} ml coffee, froth milk, and mix all ingredients for a cappuccino");
            return new Cappuccino();
        }
    }

    // Class for creating hot chocolate
    internal class HotChocolateFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pouring {total} ml of hot water into the cup, then mixing in some chocolate powder then it's ready to serve");
            return new HotChocolate();
        }
    }

    // Main class for the coffee machine
    public class WarmDrinkMachine
    {
        // Enum for available drinks
        public enum AvailableDrink
        {
            Coffee, Cappuccino, HotChocolate
        }

        private Dictionary<AvailableDrink, IWarmDrinkFactory> factories = new Dictionary<AvailableDrink, IWarmDrinkFactory>();

        private List<Tuple<string, IWarmDrinkFactory>> namedFactories = new List<Tuple<string, IWarmDrinkFactory>>();

        public WarmDrinkMachine()
        {
            // The constructor creates a list of available drinks based on factory classes implementing IWarmDrinkFactory
            foreach (var t in typeof(WarmDrinkMachine).Assembly.GetTypes())
            {
                // Check if the type implements the IWarmDrinkFactory interface and is not an interface itself
                if (typeof(IWarmDrinkFactory).IsAssignableFrom(t) && !t.IsInterface)
                {
                    // Add the factory name (without "Factory" suffix) and an instance of the factory to the namedFactories list
                    namedFactories.Add(Tuple.Create(
                        t.Name.Replace("Factory", string.Empty), (IWarmDrinkFactory)Activator.CreateInstance(t)));
                }
            }
        }

        public IWarmDrink MakeDrink()
        {
            // Display the available drinks in the menu
            Console.WriteLine("This is what we serve today:");
            for (var index = 0; index < namedFactories.Count; index++)
            {
                var tuple = namedFactories[index];
                Console.WriteLine($"{index + 1}: {GetFormattedName(tuple.Item1)}");
            }

            // Prompt the user to select a drink from the menu
            Console.WriteLine("Select a number to continue:");
            while (true)
            {
                string s;
                if ((s = Console.ReadLine()) != null
                    && int.TryParse(s, out int i)
                    && i >= 1                      
                    && i <= namedFactories.Count)
                {
                    // Prompt the user to enter the amount of the selected drink
                    Console.Write("How much do you want of your product, in ml? ");
                    s = Console.ReadLine();
                    if (s != null
                        && int.TryParse(s, out int total)
                        && total > 0)
                    {
                        // Create and return the selected drink
                        return namedFactories[i - 1].Item2.Prepare(total);
                    }
                }
                Console.WriteLine("Something went wrong with your input, try again.");
            }
        }

        // Method for formatting the name of the drink to be displayed in the menu
        private string GetFormattedName(string name)
        {
            switch (name)
            {
                case "HotWater":
                    return "Hot Water";
                case "HotChocolate":
                    return "Hot Chocolate";
                default:
                    return name;
            }
        }
    }

    // Main program
    class Program
    {
        static void Main(string[] args)
        {
            var machine = new WarmDrinkMachine();
            IWarmDrink drink = machine.MakeDrink();
            drink.Consume();
        }
    }
}
