using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extensions;

namespace Test_Extensions
{
    public class Test
    {
        public static void printTwoLines()
        {
            Console.WriteLine("\n");
            //Console.WriteLine("\n");
        }

        public static void println(string s)
        {
            Console.WriteLine(s);
        }

        public static void print(string s)
        {
            Console.Write(s);
        }

        public static List<TestObject> getArray()
        {
            List<TestObject> arr = new List<TestObject>();
            arr.Push(new TestObject() { a = "mike", b = "ikechi", c = "stars", x = 1, y = 7, z = Math.PI })
                .Push(new TestObject() { a = "xxx", b = "yyy", c = "sss", x = 3, y = 2, z = Math.E })
                .Push(new TestObject() { a = "gg", b = "hh", c = "ver", x = 4, y = 2, z = Math.Log10(3) })
                .Push(new TestObject() { a = "john", b = "peter", c = "kola", x = 6, y = 72, z = Math.Cos(60) })
                .Push(new TestObject() { a = "bull", b = "jot", c = "sword", x = 3, y = 23, z = Math.PI });
            return arr;
        }

        public static List<TestObject> testPush()
        {
            println("Testing List<object>().Push() ... ");
            var arr = getArray();
            println(arr.ToJson());
            println("End Testing List<object>().Push() ... ");
            printTwoLines();
            return arr;
        }

        public static TestObject testRandomSingle()
        {
            println("Testing List<object>().Random() ... ");
            TestObject t = getArray().Random();
            println(t.ToJson());
            println("End Testing List<object>().Random() ... ");
            printTwoLines();
            return t;
        }

        public static List<TestObject> testRandom()
        {
            println("Testing List<object>().Random(count) ... ");
            List<TestObject> arr = (List<TestObject>)getArray().Random(2);
            println(arr.ToJson());
            println("End Testing List<object>().Random(count) ... ");
            return arr;
        }

        public static void testSort()
        {
            var arr = getArray();
            arr = arr.Sort("a", "asc");
            println("Print [a] in asc");
            arr.ForEach((TestObject t) => println(t.ToJson()));

            arr = arr.Sort("b", "desc");
            println("Print [b] in desc");
            arr.ForEach((TestObject t) => println(t.ToJson()));

            arr = arr.Sort("c", "asc");
            println("Print [c] in asc");
            arr.ForEach((TestObject t) => println(t.ToJson()));

            arr = arr.Sort("x", "asc");
            println("Print [x] in asc");
            arr.ForEach((TestObject t) => println(t.ToJson()));

            printTwoLines();
        }

        public static void testSelect()
        {
            println("Test for List<object>().Select(prop) ... ");
            var arr = getArray();
            println("List of [a] ... ");
            println(arr.Select("a").ToJson());
            println("List of [b] ... ");
            println(arr.Select<TestObject, string>("b").ToJson());
            printTwoLines();
        }

        public static void testJoin()
        {
            var arr = getArray().Select("a");
            println("Test for List<string>().Join() ...");
            println(arr.Join());
            println(arr.Join(" --- "));
            println(arr.Join(","));
            println(arr.Join(" --- suck --- "));
            println("End Test for List<string>().Join() ... ");
            printTwoLines();
        }

        public static void testSum()
        {
            var arr = getArray().Select("x");
            println("Test List<int>().Sum() ... ");
            println(Convert.ToString(arr.Sum()));
            println("End Test List<int>().Sum() ... ");
            printTwoLines();
        }
        
        public static void testFrom()
        {
            var arr = getArray();
            println("Test List<object>().From(index) ... ");
            println(arr.From(3).ToJson());
            println("End Test List<object>().From(index) ... ");
            printTwoLines();
        }

        public static void testWhere()
        {
            var arr = getArray();
            println("Test List<object>().Where(prop, val) ... ");
            println(arr.Where("a", "mike").ToJson());
            println("End Test List<object>().Where(prop, val) ... ");
            printTwoLines();

            println("Test List<object>().Where(func) ... ");
            println(arr.Where((TestObject t) => t.x > 3).ToJson());
            println("End Test List<object>().Where(func) ... ");
            printTwoLines();
        }

        public static void testFlatten()
        {
            var arr = new List<List<TestObject>>();
            arr.Push(getArray()).Push(getArray()).Push(getArray()).Push(getArray());
            println("Test List<List<object>>().Flatten() ... ");
            println(arr.Flatten().ToJson());
            println("End Test List<List<object>>().Flatten() ... ");
            printTwoLines();
        }

        public static void testFirst()
        {
            println(getArray().First(10).ToJson());
            println(getArray().First(2).ToJson());
            println(getArray().First().ToJson());
            printTwoLines();
        }

        public static void testLast()
        {
            println(getArray().Last(10).ToJson());
            println(getArray().Last(2).ToJson());
            println(getArray().Last().ToJson());
            printTwoLines();
        }

        public static void testPushRange()
        {
            println(getArray().PushRange(getArray().PushRange(getArray())).PushRange(null).Count.ToString());
            printTwoLines();
        }

        public static void testContains()
        {
            println(Convert.ToString(new List<string>().Push("a").Push("b").Push("c").Push("d").Contains("ab")));
            printTwoLines();
        }

        public static void testDistinct()
        {
            println(new List<string>().Push("a").Push("a").Push("c").Push("d").Distinct().ToJson());
            printTwoLines();
            println(getArray().Distinct("y").ToJson());
        }

        public static void testPaginate()
        {
            println(getArray().PushRange(getArray()).PushRange(getArray()).Paginate(4).Count.ToJson());
        }

    }

    public class TestObject
    {
        public string a { get; set; }
        public string b { get; set; }
        public string c { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public double z { get; set; }
        public DeepTestObject dt { get; set; }
    }

    public class DeepTestObject
    {
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
    }
}
