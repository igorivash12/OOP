using Lab3.Models;

namespace Lab3.Models
{
    public abstract class Product : BaseEntity
    {
        public double Price { get; set; }
    }
}