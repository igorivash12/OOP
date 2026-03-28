using System;

namespace Lab2.Shapes1
{
    /// <summary>
    /// Display name in the tool list. If omitted, the class name is used with the Shape suffix removed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ShapeDisplayNameAttribute : Attribute
    {
        public string Name { get; }

        public ShapeDisplayNameAttribute(string name)
        {
            Name = name;
        }
    }
}
