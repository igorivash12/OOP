using Lab3.Models;
using System.Collections.Generic;
using System.IO;

namespace Lab3.Services
{
    public static class TextSerializer
    {
        public static void Save(string path, List<BaseEntity> items)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (var item in items)
                {
                    sw.WriteLine(item.Serialize());
                }
            }
        }

        public static List<BaseEntity> Load(string path)
        {
            var list = new List<BaseEntity>();

            foreach (var line in File.ReadAllLines(path))
            {
                BaseEntity obj = Create(line);
                obj.Deserialize(line);
                list.Add(obj);
            }

            return list;
        }

        private static BaseEntity Create(string line)
        {
            string type = line.Split('|')[0];

            switch (type)
            {
                case "LuxuryPerfume":
                    return new LuxuryPerfume();
                case "BudgetPerfume":
                    return new BudgetPerfume();
                case "Cosmetic":
                    return new Cosmetic();
                default:
                    return null;
            }
        }
    }
}