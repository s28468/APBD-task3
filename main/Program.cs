using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Container
{
    private static int _nextUniqueNumber = 0;
    private static readonly HashSet<string> _existingNumbers = new HashSet<string>();

    public abstract int cargo_mass { get; set; }
    public abstract int height { get; set; }
    public abstract int tare_weight { get; set; }
    public abstract int depth { get; set; }
    public abstract string number { get; set; }
    public abstract int max_payload { get; set; }
    public abstract string ContainerType { get; }
    private string _number;
    public string Number
    {
        get => _number;
        set
        {
            string potentialNumber = $"KON-{ContainerType}-{_nextUniqueNumber}";
            while (_existingNumbers.Contains(potentialNumber))
            {
                potentialNumber = $"KON-{ContainerType}-{_nextUniqueNumber++}";
            }
            
            _existingNumbers.Add(potentialNumber);
            _number = potentialNumber;
        }
    }
    protected Container()
    {
        generate_unique_number();
    }
    private void generate_unique_number()
    {
        string potential_number;
        do
        {
            potential_number = $"KON-{ContainerType}-{_nextUniqueNumber++}";
        } while (_existingNumbers.Contains(potential_number));
        
        _existingNumbers.Add(potential_number);
        Number = potential_number;
    }

    public virtual void Emptying()
    {
        cargo_mass = 0;
    }
    public virtual void Load(int new_cargo_mass)
    {
        if (new_cargo_mass > max_payload)
        {
            throw new OverflowException("The cargo mass exceeds the container's capacity.");
        }

        cargo_mass = new_cargo_mass;
    }
}

public interface IHazardNotifier
{
    void SendHazardNotification(string message);
}

public class LContainer : Container, IHazardNotifier
{
    public override string ContainerType => "L";
    private static int _nextUniqueNumber = 1;
    private static readonly HashSet<string> _existingNumbers = new HashSet<string>();
    public bool ContainsHazardousMaterial { get; set; }
    public override int cargo_mass { get; set; }
    public override int height { get; set; }
    public override int tare_weight { get; set; }
    public override int depth { get; set; }
    public override string number { get; set; }
    public override int max_payload { get; set; }
    private  string _number;

    public  string Number
    {
        get => _number;
        private set
        {
            _number = value; 
        }
    }
    public LContainer()
    {
        generate_unique_number();
    }
    private void generate_unique_number()
    {
        string potential_number;
        do
        {
            potential_number = $"KON-L-{_nextUniqueNumber++}";
        } while (_existingNumbers.Contains(potential_number));
        
        _existingNumbers.Add(potential_number);
        Number = potential_number;
    }
    

    public void SendHazardNotification(string message)
    {
        Console.WriteLine($"Hazard notification for {number}: {message}");
    }

    public override void Load(int cargoMass)
    {
        int allowed_сapacity = ContainsHazardousMaterial ? max_payload / 2 : (int)(max_payload * 0.9);

        if (cargoMass > allowed_сapacity)
        {
            SendHazardNotification("Attempt to exceed the allowed cargo capacity.");
            throw new InvalidOperationException("Cannot load cargo: capacity exceeded.");
        }
        else
        {
            cargo_mass = cargoMass;
        }
    }
}

public class GContainer : Container, IHazardNotifier
{
    public override string ContainerType => "G";

    private static int _nextUniqueNumber = 1;
    private static readonly HashSet<string> _existingNumbers = new HashSet<string>();
    public double Pressure { get; set; }
    public override int cargo_mass { get; set; }
    public override int height { get; set; }
    public override int tare_weight { get; set; }
    public override int depth { get; set; }
    public override string number { get; set; }
    public override int max_payload { get; set; }

    private  string _number;
    public  string Number
    {
        get => _number;
        private set
        {
            _number = value; 
        }
    }
    public GContainer()
    {
        generate_unique_number();
    }
    private void generate_unique_number()
    {
        string potential_number;
        do
        {
            potential_number = $"KON-{ContainerType}-{_nextUniqueNumber++}";
        } while (_existingNumbers.Contains(potential_number));
        
        _existingNumbers.Add(potential_number);
        Number = potential_number;
    }

    public void SendHazardNotification(string message)
    {
        Console.WriteLine($"Hazard notification for {number}: {message}");
    }

    public override void Emptying()
    {
        cargo_mass = (int)(max_payload * 0.05);
    }

    public override void Load(int cargoMass)
    {
        if (cargoMass > max_payload)
        {
            SendHazardNotification("Attempt to exceed the allowed cargo capacity.");
            throw new InvalidOperationException("Cannot load cargo: capacity exceeded.");
        }
        else
        {
            max_payload = cargoMass;
        }
    }
}

public class RContainer : Container
{
    public override string ContainerType => "R";

    private static int _nextUniqueNumber = 1;
    private static readonly HashSet<string> _existingNumbers = new HashSet<string>();
    public override int cargo_mass { get; set; }
    public override int height { get; set; }
    public override int tare_weight { get; set; }
    public override int depth { get; set; }
    public override string number { get; set; }
    public override int max_payload { get; set; }
    public string product_type { get; private set; }
    public double Temperature { get; private set; }


    private  string _number;
    public  string Number
    {
        get => _number;
        private set
        {
            _number = value; 
        }
    }
    public RContainer()
    {
        generate_unique_number();
    }
    private void generate_unique_number()
    {
        string potential_number;
        do
        {
            potential_number = $"KON-{ContainerType}-{_nextUniqueNumber++}";
        } while (_existingNumbers.Contains(potential_number));
        
        _existingNumbers.Add(potential_number);
        Number = potential_number;
    }
    private double GetRequiredTemperature(string productType)
    {
        Dictionary<string, double> productTemperatures = new Dictionary<string, double>
        {
            {"Bananas", 13.3},
            {"Chocolate", 18},
            {"Fish", 2},
            {"Meat", -15},
            {"Ice cream", -18},
            {"Frozen pizza", -30},
            {"Cheese", 7.2},
            {"Sausages", 5},
            {"Butter", 20.5},
            {"Eggs", 19}
        };

        if (productTemperatures.TryGetValue(productType, out double temperature))
        {
            return temperature;
        }
        throw new ArgumentException("Unknown product type", nameof(productType));
    }

    public void SetTemperature(double temperature)
    {
        if (temperature < GetRequiredTemperature(product_type))
        {
            throw new InvalidOperationException("Temperature is too low for the stored product type.");
        }

        Temperature = temperature;
    }
    

    public override void Emptying()
    {
        cargo_mass = 0;
    }
    public override void Load(int new_cargo_mass)
    {
        if (new_cargo_mass > max_payload)
        {
            throw new OverflowException("The cargo mass exceeds the container's capacity.");
        }

        cargo_mass = new_cargo_mass;
    }
}

public class Ship
{
    public List<Container> Containers { get; set; } = new List<Container>();
    public int max_speed { get; set; }
    public int max_container_count { get; set; }
    public double max_weight { get; set; }

    public void LoadContainer(Container container)
    {
        if (Containers.Count < max_container_count &&
            Containers.Sum(c => c.cargo_mass) + container.cargo_mass <= max_weight)
        {
            Containers.Add(container);
        }
        else
        {
            throw new InvalidOperationException("Cannot load container: limit reached.");
        }
    }

    public void LoadContainers(List<Container> containers)
    {
        foreach (var container in containers)
        {
            LoadContainer(container);
        }
    }

    public void UnloadContainer(Container container)
    {
        Containers.Remove(container);
    }

    public void ReplaceContainer(string number, Container newContainer)
    {
        var containerToReplace = Containers.FirstOrDefault(c => c.number == number);
        if (containerToReplace != null)
        {
            UnloadContainer(containerToReplace);
            LoadContainer(newContainer);
        }
    }

    public void PrintShipInfo()
    {
        Console.WriteLine(
            $"Ship max speed: {max_speed} knots, max containers: {max_container_count}, max weight: {max_weight} tons");
        Console.WriteLine("Containers on board:");
        foreach (var container in Containers)
        {
            Console.WriteLine($"- Type: {container.Number}, Cargo Mass: {container.cargo_mass}");
        }
    }
}

public class Program
{
    public static void Main()
    {
        var ship = new Ship { max_speed = 20, max_container_count = 10, max_weight = 200000 };

        var container1 = new RContainer {};
        var container2 = new LContainer {};
        var container3 = new GContainer {};
        container1.max_payload = 20000;
        container2.max_payload = 17000;
        container3.max_payload = 9000;
        container1.Load(1000);
        container2.Load(15000);
        container3.Load(5000);
        try
        {
            ship.LoadContainer(container1);
            ship.LoadContainer(container2);
            ship.LoadContainers(new List<Container> { container3 });

            ship.PrintShipInfo();

            ship.UnloadContainer(container2);
            ship.ReplaceContainer("Dry", new LContainer {});

            ship.PrintShipInfo();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}