# Om uppgiften

I denna uppgift kommer du få en färdig enklare applikation som du sedan ska vidareutveckla med lite mer funktionalitet. Du ska i denna vidareutveckling fortsätta använda samma design mönster som redan används i applikationen.

# Vad du ska göra

- [ ]  Kopiera koden under till ditt projekt
    - **Kod att kopiera** (klicka på pilen för att fälla ut)
        
        ```csharp
        using System;
        using System.Collections.Generic;
        
        namespace VarmDrinkStation
        {
            public interface IWarmDrink
            {
                void Consume();
            }
            internal class Water : IWarmDrink
            {
                public void Consume()
                {
                    Console.WriteLine("Warm water is served.");
                }
            }
            public interface IWarmDrinkFactory
            {
                IWarmDrink Prepare(int total);
            }
            internal class HotWaterFactory : IWarmDrinkFactory
            {
                public IWarmDrink Prepare(int total)
                {
                    Console.WriteLine($"Pour {total} ml hot water in your cup");
                    return new Water();
                }
            }
            public class WarmDrinkMachine
            {
                public enum AvailableDrink // violates open-closed
                {
                    Coffee, Tea
                }
                private Dictionary<AvailableDrink, IWarmDrinkFactory> factories =
                  new Dictionary<AvailableDrink, IWarmDrinkFactory>();
        
                private List<Tuple<string, IWarmDrinkFactory>> namedFactories =
                  new List<Tuple<string, IWarmDrinkFactory>>();
        
                public WarmDrinkMachine()
                {
                    foreach (var t in typeof(WarmDrinkMachine).Assembly.GetTypes())
                    {
                        if (typeof(IWarmDrinkFactory).IsAssignableFrom(t) && !t.IsInterface)
                        {
                            namedFactories.Add(Tuple.Create(
                              t.Name.Replace("Factory", string.Empty), (IWarmDrinkFactory)Activator.CreateInstance(t)));
                        }
                    }
                }
                public IWarmDrink MakeDrink()
                {
                    Console.WriteLine("This is what we serve today:");
                    for (var index = 0; index < namedFactories.Count; index++)
                    {
                        var tuple = namedFactories[index];
                        Console.WriteLine($"{index}: {tuple.Item1}");
                    }
                    Console.WriteLine("Select a number to continue:");
                    while (true)
                    {
                        string s;
                        if ((s = Console.ReadLine()) != null
                            && int.TryParse(s, out int i) // c# 7
                            && i >= 0
                            && i < namedFactories.Count)
                        {
                            Console.Write("How much: ");
                            s = Console.ReadLine();
                            if (s != null
                                && int.TryParse(s, out int total)
                                && total > 0)
                            {
                                return namedFactories[i].Item2.Prepare(total);
                            }
                        }
                        Console.WriteLine("Something went wrong with your input, try again.");
                    }
                }
            }
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
        ```
        
- [ ]  Testa först att du kan köra applikationen i befintligt skick
- [ ]  Bygg sedan till funktioner för att tillaga/bereda kaffe, cappuccino och choklad.
- [ ]  Du ska fortsätta använda samma design mönster som redan finns i applikationen och utöka det utan att lägga till fler design mönster eller sluta använda dem
