using System;
using System.Collections.Generic;

namespace StoreKata
{
    public class ItemManager
    {

        private List<Item> items = new List<Item>();
        private float totalPrice = 0.0f;
        private string markedDownText = "";

        public struct Item
        {
            // Fixed
            public string name;
            public float quanity;

            public float value;

            // Updates after scan
            public float price;

            public float markDownAmount;

            public Type type;
            public enum Type { Weighed, Each };
        }

        public void RunTest()
        {

            // Test Items
            Item testItem1;
            testItem1.name = "Milk";
            testItem1.quanity = 1;
            testItem1.value = 2.50f;
            testItem1.price = 0.0f;
            testItem1.type = Item.Type.Each;
            testItem1.markDownAmount = 0.2f;

            Item testItem2;
            testItem2.name = "Soup";
            testItem2.quanity = 6;
            testItem2.value = 2.70f;
            testItem2.price = 0.0f;
            testItem2.type = Item.Type.Each;
            testItem2.markDownAmount = 1.0f;

            Item testItem3;
            testItem3.name = "Bananas";
            testItem3.quanity = 0.4f;
            testItem3.value = 1.89f;
            testItem3.price = 0.0f;
            testItem3.type = Item.Type.Weighed;
            testItem3.markDownAmount = 0.0f;
            
            testItem1 = Markdown(testItem1, testItem1.markDownAmount);
            ScanItem(testItem1);

            testItem2 = Markdown(testItem2, testItem2.markDownAmount);
            ScanItem(testItem2);

            testItem3 = Markdown(testItem3, testItem3.markDownAmount);
            ScanItem(testItem3);

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

        // Markdown Item Test
        public Item Markdown(Item storeItem, float amount)
        {
            if (amount <= 0.0f)
                return storeItem;

            Item item = storeItem;
            item.value -= amount;

            markedDownText = string.Format("\tMarked-Down from $ {0 :}!", storeItem.value);

            return item;
           
        }

        // Display Methods
        private void UpdateCheckout(Item item)
        {
            if (item.type == Item.Type.Each)
            {
                if(item.quanity > 1)
                    Console.WriteLine("- " + item.name + "\t\t" + item.quanity + "X" + " ($" + item.value + " each) " + "\n\t\t\t\t\t$" + item.price);
                else
                    Console.WriteLine("- " + item.name + "\t\t\t\t\t$" + item.price);
            }
            else
            {
                Console.WriteLine("- " + item.name + "\t" + item.quanity + "lbs." + " ($" + item.value + " / lb.) " + "\n\t\t\t\t\t$" + item.price);
            }

            if(item.markDownAmount > 0.0f)
                Console.WriteLine(markedDownText);


            Console.WriteLine("------------------------------------------------------");

        }

        private void DisplayCheckout()
        {
            Console.WriteLine("\n\t\t\t\t Total: $" + totalPrice);
        }

    }
}
