using System;

namespace Lab3.Models
{
    public abstract class BaseEntity
    {
        public string Name { get; set; }

        public abstract string Serialize();
        public abstract void Deserialize(string data);
    }
}