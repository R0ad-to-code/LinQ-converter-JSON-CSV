using Newtonsoft.Json.Linq;
using System.Linq;
using LinQ_converter_JSON_CSV.classes;

// import fichier JSON en instance de classe
var myJsonFruits = JObject.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + "/assets/data.json"));
var fruits = new FruitList
{
    Fruits = myJsonFruits["AllFruits"].ToObject<List<Fruit>>(),
    IsModified = false
};
// copie de l'instance pour pouvoir modifier l'affichage sans toucher à l'instance d'origine
var fruitsListToExport = new FruitList
{
    Fruits = fruits.Fruits.Select(f => new Fruit
    {
        Name = f.Name,
        Color = f.Color,
        Season = f.Season,
        Continent = f.Continent,
        Taste = f.Taste
    }).ToList(),
    IsModified = false
};
// interaction utilisateur
Console.WriteLine("Bienvenue dans notre application de gestion de fruits !\n");
PrintList(fruitsListToExport);
Console.WriteLine("Souhaitez-vous modifier l'affichage des données avant de les exporter ? (O/N)");
// tant que l'utilisateur veut faire des modifications, on lui propose de choisir entre trier, grouper ou faire une recherche
while (Console.ReadLine()?.ToUpper() == "O")
{
    // possibilité de restaurer la liste initiale
    if (fruitsListToExport.IsModified == true)
    {
        fruitsListToExport = ResetList(fruits, fruitsListToExport);
    }
    fruitsListToExport = GetModificationChoice(fruitsListToExport);
    PrintList(fruitsListToExport); 
    Console.WriteLine("Souhaitez-vous modifier l'affichage des données avant de les exporter ? (O/N)");
}  
ExportCSVFruitList(fruitsListToExport, Directory.GetCurrentDirectory() + "/assets/modified_data.csv");


// Methode pour spécifier quelle modification sur la liste
FruitList GetModificationChoice(FruitList fruits) 
{
    Console.WriteLine("Souhaitez-vous trier, grouper ou faire une recherche ? (T/G/R)");
    var choice = Console.ReadLine()?.ToUpper();
    if (choice == "T")
    {
        fruits.Fruits = FilterFruits(fruits.Fruits);
        fruits.IsModified = true;
    }
    else if (choice == "G")
    {
        fruits.Fruits = GroupFruits(fruits.Fruits);
        fruits.IsModified = true;
    }
    else if (choice == "R")
    {
        fruits.Fruits = SearchFruits(fruits.Fruits);
        fruits.IsModified = true;
    }
    else
    {
        Console.WriteLine("Choix invalide. Aucune modification effectuée.");
    }
    return fruits;
}

// Methode pour grouper les fruits selon le choix de l'utilisateur
List<Fruit> GroupFruits(List<Fruit> fruits) 
{
    Console.WriteLine("Souhaitez-vous grouper par nom, couleur, saison, continent ou goût ? (N/C/S/Co/G)");
    var sortChoice = Console.ReadLine()?.ToUpper();
    switch (sortChoice)
    {
        case "N":
            return fruits.OrderByName();
        case "C":
            return fruits.OrderByColor();
        case "S":
            return fruits.OrderBySeason();
        case "CO":
            return fruits.OrderByContinent();
        case "G":
            return fruits.OrderByTaste();
        default:
            Console.WriteLine("Choix invalide. Aucun groupage effectué.");
            break;
    }
    return fruits;
}

// Methode pour filtrer les fruits selon le choix de l'utilisateur
List<Fruit> FilterFruits(List<Fruit> fruits) 
{
    Console.WriteLine("Souhaitez-vous filtrer par nom, couleur, saison, continent ou goût ? (N/C/S/Co/G)");
    var filterChoice = Console.ReadLine()?.ToUpper();
    Console.WriteLine("Entrez la valeur de filtrage : ");
    var filterValue = Console.ReadLine();
    switch (filterChoice)
    {
        case "N":
            return fruits.Where(f => f.Name.Contains(filterValue, StringComparison.InvariantCultureIgnoreCase)).ToList();
        case "C":
            return fruits.Where(f => f.Color.Contains(filterValue, StringComparison.InvariantCultureIgnoreCase)).ToList();
        case "S":
            return fruits.Where(f => f.Season.Contains(filterValue, StringComparison.InvariantCultureIgnoreCase)).ToList();
        case "CO":
            return fruits.Where(f => f.Continent.Contains(filterValue, StringComparison.InvariantCultureIgnoreCase)).ToList();
        case "G":
            return fruits.Where(f => f.Taste.Contains(filterValue, StringComparison.InvariantCultureIgnoreCase)).ToList();
        default:
            Console.WriteLine("Choix invalide. Aucun filtrage effectué.");
            break;
    }
    return fruits;
}

List<Fruit> SearchFruits(List<Fruit> fruits) 
{
    Console.WriteLine("Entrez la valeur de recherche : ");
    var searchValue = Console.ReadLine();
    return fruits.Where(f => f.Name.Contains(searchValue, StringComparison.InvariantCultureIgnoreCase)).ToList();
}

static void PrintList (FruitList fruits) 
{
    Console.WriteLine("Voici nos fruits :\n");
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    foreach (var fruit in fruits.Fruits)
    {
        Console.WriteLine($"- {fruit.Name} ({fruit.Color} - {fruit.Season} - {fruit.Continent} - {fruit.Taste})");
    }
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("\n");
}

// Methode pour restaurer la liste initiale si l'utilisateur le souhaite
static FruitList ResetList(FruitList originalFruits, FruitList modifiedFruits) 
{
    Console.WriteLine("Souhaitez-vous restaurer la liste initiale ? (O/N)");
    if (Console.ReadLine()?.ToUpper() == "O")
    {
        var resetList = new FruitList
        {
            Fruits = originalFruits.Fruits.Select(f => new Fruit
            {
                Name = f.Name,
                Color = f.Color,
                Season = f.Season,
                Continent = f.Continent,
                Taste = f.Taste
            }).ToList(),
            IsModified = false
        };
        Console.WriteLine("Votre liste de fruits a été réinitialisée."); 
        return resetList;
    }
    return modifiedFruits;
}

static void ExportCSVFruitList(FruitList fruits, string filePath) 
{
    using (var writer = new StreamWriter(filePath))
    {
        writer.WriteLine("Name,Color,Season,Continent,Taste");
        foreach (var fruit in fruits.Fruits)
        {
            writer.WriteLine($"{fruit.Name},{fruit.Color},{fruit.Season},{fruit.Continent},{fruit.Taste}");
        }
    }
    Console.WriteLine("Liste de fruits exportée au format CSV avec succès !");
}