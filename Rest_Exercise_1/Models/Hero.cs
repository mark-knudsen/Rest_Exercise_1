namespace Rest_Exercise_1.Models
{
    public class Hero
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public List<string> inventory { get; set; }
        public bool isCoolHero { get; set; }
    }
}
