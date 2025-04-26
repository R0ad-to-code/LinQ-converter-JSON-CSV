namespace LinQ_converter_JSON_CSV.classes
{
    public class Fruit
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public string Season { get; set; }
        public string Continent { get; set; }
        public string Taste { get; set; }
    }

    public class FruitList
    {
        public List<Fruit> Fruits { get; set; }
        public Boolean IsModified { get; set; }
    }
     public static class FruitExtensions
    {
        public static List<Fruit> OrderByName(this List<Fruit> fruits)
        {
            return fruits.OrderBy(f => f.Name).ToList();
        }

        public static List<Fruit> OrderByColor(this List<Fruit> fruits)
        {
            return fruits.OrderBy(f => f.Color).ToList();
        }

        public static List<Fruit> OrderBySeason(this List<Fruit> fruits)
        {
            return fruits.OrderBy(f => f.Season).ToList();
        }

        public static List<Fruit> OrderByContinent(this List<Fruit> fruits)
        {
            return fruits.OrderBy(f => f.Continent).ToList();
        }

        public static List<Fruit> OrderByTaste(this List<Fruit> fruits)
        {
            return fruits.OrderBy(f => f.Taste).ToList();
        }
    }
}