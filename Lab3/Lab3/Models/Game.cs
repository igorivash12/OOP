namespace Lab3.Models
{
    public abstract class Game : BaseEntity
    {
        public double Price { get; set; }
        public string Genre { get; set; }
    }
}