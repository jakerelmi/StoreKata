using System;

namespace StoreKata
{
    class Program
    {
        static void Main(string[] args)
        {
            ItemManager itemManager = new ItemManager();
            itemManager.RunTest();

            Console.WriteLine("\n\n\n\nPress any key to close...");
            Console.ReadKey();
        }
    }

}
