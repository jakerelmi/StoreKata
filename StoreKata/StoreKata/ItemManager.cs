using System;
using System.Collections.Generic;

namespace StoreKata
{
    public class ItemManager
    {

        private List<Item> items = new List<Item>();
        private float totalPrice = 0.0f;

        public struct Item
        {
            // Fixed
            public string name;
            public int quanity;

            public float value;

            // Updates after scan
            public float price;
        }

        public void RunTest()
        {

            // Test Items
            Item testItem1;
            testItem1.name = "Milk";
            testItem1.quanity = 1;
            testItem1.value = 2.50f;
            testItem1.price = 0.0f;

            Item testItem2;
            testItem2.name = "Soup";
            testItem2.quanity = 4;
            testItem2.value = 2.50f;
            testItem2.price = 0.0f;

            //Add Item Test
            ScanItem(testItem1);
            ScanItem(testItem2);

            DisplayCheckout();
        }

        // Add Item Test
        public void ScanItem(Item storeItem)
        {
            storeItem.price = storeItem.value * storeItem.quanity;

            // Increment total price
            totalPrice += (storeItem.price);

            // Add item to list
            items.Add(storeItem);

            UpdateCheckout(storeItem);
        }


        // Display Methods
        private void UpdateCheckout(Item item)
        {
            Console.WriteLine("- " + item.name + "\t\t\t$" + item.price);
        }

        private void DisplayCheckout()
        {
            Console.WriteLine("\n\n\t\t Total: $" + totalPrice);
        }

    }
}
