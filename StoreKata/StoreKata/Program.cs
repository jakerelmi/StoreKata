using System;

namespace StoreKata
{
    class Program
    {
        static void Main(string[] args)
        {
            ItemManager itemManager = new ItemManager();

            itemManager.DisplayStoreOptions();
            while (true)
                itemManager.UpdateStoreUsingUserInput();
            
        }
    }

}
