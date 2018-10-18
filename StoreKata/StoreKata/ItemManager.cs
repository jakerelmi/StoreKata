using System;
using System.Collections.Generic;

namespace StoreKata
{
    public class ItemManager
    {

        private List<Item> items = new List<Item>();
        private float totalPrice = 0.0f;
        private string markedDownText = "";
        private string specialOfferText = "";

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
            testItem1.markDownAmount = 0.0f;

            Item testItem2;
            testItem2.name = "Soup";
            testItem2.quanity = 10;
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
            
            // Milk
            testItem1 = Markdown(testItem1, testItem1.markDownAmount);
            ScanItem(testItem1);

            // Soup
            testItem2 = Markdown(testItem2, testItem2.markDownAmount);
            ScanItem(testItem2);

            // Bananas
            testItem3 = Markdown(testItem3, testItem3.markDownAmount);
            ScanItem(testItem3);

            // Apply specials
            //BuyNItemsGetMOffSpecial(testItem1, 2, 0.5f, 2); // Buy 2 get next 50% off on Milk
            BuyNItemsGetMOffSpecial(testItem2, 1, 1.0f, 4); // Buy 1 get 1 free soup (Max of 4).
            BuyNforXSpecial(testItem2, 3, 2.0f); // Buy 3 for $2.00 soup

            Console.WriteLine("\n\t\t\t\t\t Total: $" + totalPrice);
        }

        // Add Item Test
        private void ScanItem(Item storeItem)
        {
            storeItem.price = storeItem.value * storeItem.quanity;

            // Increment total price
            totalPrice += (storeItem.price);

            // Add item to list
            items.Add(storeItem);

            DisplayCheckout(storeItem);
        }

        // Markdown Item Test
        private Item Markdown(Item storeItem, float amount)
        {
            if (amount <= 0.0f)
                return storeItem;

            Item item = storeItem;
            item.value -= amount;

            markedDownText = string.Format("\t-Marked-Down from ${0 :}!", storeItem.value);

            return item;
           
        }

        // Special Item Test (Buy N get M X% off)
        private void BuyNItemsGetMOffSpecial(Item storeItem, int quanitityQualification, float amount, int maximumSpecials)
        {
            // Only apply to non-weighed items
            if (storeItem.type != Item.Type.Each)
                return;

            if (storeItem.quanity > quanitityQualification)
            {
                // To accumulate specials
                int qualifications = 0;
                int index = 0;
                for (int i = 0; i < storeItem.quanity; i++)
                {
                    if (index == quanitityQualification)
                    {
                        index = 0;
                        qualifications++;

                        if (qualifications >= maximumSpecials)
                            break;

                        continue;
                    }
                    index++;

                }
                
                // Apply discount after scan 
                float discountAmount = (amount * storeItem.value) * qualifications;
                totalPrice -= discountAmount;

                // Display to customer
                specialOfferText = string.Format("-Applied Buy {0} get 1 {1}% off special for {2}!\n\t\t\t\t\t\t- ${3}", quanitityQualification, amount * 100, storeItem.name, discountAmount);
            }
            
            if (specialOfferText != "")
                Console.WriteLine(specialOfferText);
        }

        // Special Item Test (Buy N for $X)
        private void BuyNforXSpecial(Item storeItem, int quanitityQualification, float amount)
        {
            // Only apply to non-weighed items
            if (storeItem.type != Item.Type.Each)
                return;

            if (storeItem.quanity >= quanitityQualification)
            {
                 float discountAmount = (quanitityQualification * storeItem.value) - amount;
                 totalPrice -= discountAmount;

                // Display to customer
                specialOfferText = string.Format("-Applied Buy {0} for ${1} special for {2}!\n\t\t\t\t\t\t- ${3}", quanitityQualification, amount, storeItem.name, discountAmount);
            }

            if (specialOfferText != "")
                Console.WriteLine(specialOfferText);
        }

        // Display 
        private void DisplayCheckout(Item item)
        {
            if (item.type == Item.Type.Each)
            {
                if(item.quanity > 1)
                    Console.WriteLine("- " + item.name + "\t\t" + item.quanity + "X" + " ($" + item.value + " each) " + "\n\t\t\t\t\t\t$" + item.price);
                else
                    Console.WriteLine("- " + item.name + "\t\t\t\t\t\t$" + item.price);
            }
            else
            {
                Console.WriteLine("- " + item.name + "\t" + item.quanity + " lbs." + " ($" + item.value + " / lb.) " + "\n\t\t\t\t\t\t$" + item.price);
            }

            if(item.markDownAmount > 0.0f)
                Console.WriteLine(markedDownText);
            
            specialOfferText = "";
            
            Console.WriteLine("------------------------------------------------------");

        }
    }
}
