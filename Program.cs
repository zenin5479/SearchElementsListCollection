using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// Поиск в элементах коллекции List<T>

namespace SearchElementsListCollection
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var coll = new List<LiveBeings>();
            coll.Add(new Dog());
            coll.Add(new Dog());
            coll.Add(new Horse());
            coll.Add(new FishSmallfry());

            var list = coll.GetObjectsWithField("Legs").ToList();
            foreach (var e in list)
            {
                Console.WriteLine(e.GetType().Name);
            }

            Console.WriteLine("Ног {0}", list.Sum("Legs"));
            Console.WriteLine("Хвостов {0}", list.Sum("Tail"));
        }
    }

    static class FilterList
    {
        public static int Sum(this IEnumerable<LiveBeings> source, string field)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var num =
                source.GetObjectsWithField(field).Sum(leg => (int)(dynamic)leg.GetType().GetField(field).GetValue(leg));

            return num;
        }

        public static IEnumerable<T> GetObjectsWithField<T>(this IEnumerable<T> list, string fieldName)
        {
            return list.Where(e => HasField(e, fieldName));
        }

        private static bool HasField<T>(T element, string fieldName)
        {
            var t = element.GetType();

            return t.GetFields(BindingFlags.Public | BindingFlags.Instance).Any(f => f.Name == fieldName);
        }
    }

    class FishSmallfry : LiveBeings
    {
        public int Tail = 1;
    }

    class Crucian : LiveBeings
    {
        public int Tail = 1;
    }

    class Dog : LiveBeings
    {
        public int Legs = 4;
        public double Tail = 1;
    }

    class Horse : LiveBeings
    {
        public int Legs = 4;
        public long Tail = 1;
    }

    abstract class LiveBeings { }
}