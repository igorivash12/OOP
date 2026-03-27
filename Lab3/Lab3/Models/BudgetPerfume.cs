using Lab3.Models;
using System.Diagnostics;
using System.Xml.Linq;

namespace Lab3.Models
{
    public class BudgetPerfume : Perfume
    {
        public override string Serialize()
        {
            return $"BudgetPerfume|{Name}|{Price}|{Volume}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            Name = parts[1];
            Price = double.Parse(parts[2]);
            Volume = int.Parse(parts[3]);
        }
    }
}