namespace Lab4.Models
{
    /// <summary>Intermediate hierarchy level: all games share price and genre.</summary>
    public abstract class Game : BaseEntity
    {
        public double Price { get; set; }
        public string Genre { get; set; }
    }
}