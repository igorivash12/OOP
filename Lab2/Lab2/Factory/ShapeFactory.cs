using Lab2.Shapes1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace Lab2.Factory
{
    // Factory: concrete shapes are discovered via reflection (new shapes need no Form1 changes).
    public class ShapeFactory
    {
        private readonly Dictionary<string, Func<Point, Point, Shape>> creators
            = new Dictionary<string, Func<Point, Point, Shape>>();

        public ShapeFactory()
            : this(typeof(Shape).Assembly)
        {
        }

        public ShapeFactory(Assembly assembly)
        {
            RegisterConcreteShapes(assembly);
        }

        /// <summary>
        /// Registers all non-abstract <see cref="Shape"/> subclasses with a (Point, Point) constructor.
        /// UI name comes from <see cref="ShapeDisplayNameAttribute"/> or the class name without the Shape suffix.
        /// </summary>
        public void RegisterConcreteShapes(Assembly assembly)
        {
            foreach (Type type in GetLoadableTypes(assembly))
            {
                if (!typeof(Shape).IsAssignableFrom(type) || type.IsAbstract || !type.IsClass)
                    continue;

                ConstructorInfo ctor = type.GetConstructor(new[] { typeof(Point), typeof(Point) });
                if (ctor == null)
                    continue;

                string name = GetDisplayName(type);
                creators[name] = (s, e) => (Shape)Activator.CreateInstance(type, s, e);
            }
        }

        public void Register(string name, Func<Point, Point, Shape> creator)
        {
            creators[name] = creator;
        }

        public Shape Create(string name, Point start, Point end)
        {
            return creators[name](start, end);
        }

        public IEnumerable<string> GetShapeNames()
        {
            return creators.Keys.OrderBy(k => k, StringComparer.OrdinalIgnoreCase);
        }

        private static string GetDisplayName(Type type)
        {
            foreach (ShapeDisplayNameAttribute attr in type.GetCustomAttributes(typeof(ShapeDisplayNameAttribute), false))
            {
                if (!string.IsNullOrEmpty(attr.Name))
                    return attr.Name;
            }

            string n = type.Name;
            const string suffix = "Shape";
            if (n.EndsWith(suffix, StringComparison.Ordinal) && n.Length > suffix.Length)
                return n.Substring(0, n.Length - suffix.Length);
            return n;
        }

        private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(t => t != null);
            }
        }
    }
}
