using System;

namespace StoreKata
{
    class Program
    {
        static void Main(string[] args)
        {
            ItemManager itemManager = new ItemManager();
            //itemManager.RunTest();

            itemManager.DisplayStoreOptions();
            while (true)
                itemManager.UpdateStoreUsingUserInput();
            
            Console.ReadKey();
        }
    }

}
