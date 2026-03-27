using Lab3.Models;
using System.Diagnostics;
using System.Xml.Linq;

namespace Lab3.Models
{
    public class Cosmetic : Product
    {
        public string Type { get; set; }

        public override string Serialize()
        {
            return $"Cosmetic|{Name}|{Price}|{Type}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            Name = parts[1];
            Price = double.Parse(parts[2]);
            Type = parts[3];
        }
    }
}