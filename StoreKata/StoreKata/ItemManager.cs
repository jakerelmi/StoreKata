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

        Item testItem1, testItem2, testItem3, testItem4;

        public ItemManager()
        {
            testItem1.name = "Milk";
            testItem1.quanity = 1;
            testItem1.value = 2.50f;
            testItem1.price = 0.0f;
            testItem1.type = Item.Type.Each;
            testItem1.markDownAmount = 0.0f;
            testItem1.discountAmount = 0.0f;

            testItem2.name = "Soup";
            testItem2.quanity = 10;
            testItem2.value = 2.70f;
            testItem2.price = 0.0f;
            testItem2.type = Item.Type.Each;
            testItem2.markDownAmount = 0.0f;
            testItem2.discountAmount = 0.0f;

            testItem3.name = "Bananas";
            testItem3.quanity = 0.4f;
            testItem3.value = 1.89f;
            testItem3.price = 0.0f;
            testItem3.type = Item.Type.Weighed;
            testItem3.markDownAmount = 0.0f;
            testItem3.discountAmount = 0.0f;

            testItem4.name = "Beef";
            testItem4.quanity = 1.0f;
            testItem4.value = 1.70f;
            testItem4.price = 0.0f;
            testItem4.type = Item.Type.Weighed;
            testItem4.markDownAmount = 0.0f;
            testItem4.discountAmount = 0.0f;
        }

        public struct Item
        {
            // Fixed
            public string name;
            public float quanity;

            public float value;

            // Updates after scan
            public float price;

            public float markDownAmount;
            public float discountAmount;

            public Type type;
            public enum Type { Weighed, Each };
        }

        public void RunTest()
        {
            // Milk
            testItem1 = Markdown(testItem1, testItem1.markDownAmount);
            testItem1 = ScanItemInterface(testItem1);

            // Soup
            testItem2 = Markdown(testItem2, testItem2.markDownAmount);
            testItem2 = ScanItemInterface(testItem2);

            // Bananas
            testItem3 = Markdown(testItem3, testItem3.markDownAmount);
            testItem3 = ScanItemInterface(testItem3);

            // Beef
            testItem4 = ScanItemInterface(testItem4);

            // Apply specials
            BuyNItemsGetMOffSpecial(testItem1, 2, 0.5f, 2); // Buy 2 get next 50% off on Milk
            testItem2 = BuyNItemsGetMOffSpecial(testItem2, 1, 1.0f, 4); // Buy 1 get 1 free soup (Max of 4).
            testItem2 = BuyNGetXSpecial(testItem2, 3, 2.0f); // Buy 3 for $2.00 soup
            testItem4 = BuyNGetMForXSpecialWeightedItems(testItem3, testItem4, 0.5f); // Buy N and get M of lesser or equal value X% off

            Console.WriteLine("\n\t\t\t\t\t Total: $" + totalPrice);

            RemoveStoreItem(testItem2, 1);
        }

        public void UpdateStoreUsingUserInput()
        {
            int input;

            if (int.TryParse(Console.ReadLine(), out input))
            {
                if (input > 0 && input < 6)
                {
                    switch (input)
                    {
                        case 1:
                            ScanItemInterface(testItem1);
                            break;
                        case 2:
                            ScanItemInterface(testItem2);
                            break;

                        case 3:
                            ScanItemInterface(testItem3);
                            break;

                        case 4:
                            ScanItemInterface(testItem4);
                            break;
                        case 5:
                            if (items.Count >= 1)
                                RemoveStoreItem(items[items.Count - 1], 1);
                            else
                                Console.WriteLine("Your cart is empty!");

                            break;
                    }
                }
                else
                    Console.WriteLine("Please Enter command:\n\n");
            }
            else
            {
                Console.WriteLine("Please Enter command:\n\n");
            }
        }

        public void DisplayStoreOptions()
        {
            Console.WriteLine("WELCOME TO THE STORE");
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("Offer: Buy 1 get 1 50% off Soup special! (4 max)");
            Console.WriteLine("Offer: Buy 3 for $2.00 Milk special!\n\n");


            Console.WriteLine("Please Enter command:\n\n");
            Console.WriteLine("1. Scan Milk\t\t\t\t$" + testItem1.value);
            Console.WriteLine("2. Scan Soup\t\t\t\t$" + testItem2.value);
            Console.WriteLine("3. Scan Bananas\t\t\t\t$" + testItem3.value + " / lb.");
            Console.WriteLine("4. Scan Beef\t\t\t\t$" + testItem4.value + " / lb.");
            Console.WriteLine("5. Remove last item");
            Console.WriteLine("------------------------------------------------------\n\n");
        }

        // Add Item Test
        public Item ScanItem(Item storeItem)
        {
            Item item = storeItem;
            item.price = item.value * item.quanity;

            // Increment total price
            totalPrice += (item.price);

            // Add item to list
            items.Add(item);

            return item;
        }

        // Markdown Item Test
        public Item Markdown(Item storeItem, float amount)
        {
            if (amount <= 0.0f)
                return storeItem;

            Item item = storeItem;
            item.value -= amount;

            markedDownText = string.Format("\t-Marked-Down from ${0 :}!", storeItem.value);

            return item;

        }

        // Special Item Test (Buy N get M X% off)
        public Item BuyNItemsGetMOffSpecial(Item storeItem, int quantityQualification, float amount, int maximumSpecials)
        {
            // Only apply to non-weighed items
            if (storeItem.type != Item.Type.Each)
                return storeItem;

            Item item = storeItem;

            if (storeItem.quanity > quantityQualification)
            {
                // To accumulate specials
                int qualifications = 0;
                int index = 0;
                for (int i = 0; i < storeItem.quanity; i++)
                {
                    if (index == quantityQualification)
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
                item.discountAmount = (amount * storeItem.value) * qualifications;
                totalPrice -= item.discountAmount;

                // Display to customer
                specialOfferText = string.Format("-Applied Buy {0} get 1 {1}% off special for {2}!\n\t\t\t\t\t\t- ${3}", quantityQualification, amount * 100, storeItem.name, item.discountAmount);
            }

            if (specialOfferText != "")
                Console.WriteLine(specialOfferText);

            return item;
        }

        // Special Item Test (Buy N for $X)
        public Item BuyNGetXSpecial(Item storeItem, int quantityQualification, float amount)
        {
            // Only apply to non-weighed items
            if (storeItem.type != Item.Type.Each)
                return storeItem;

            Item item = storeItem;

            if (storeItem.quanity >= quantityQualification)
            {
                item.discountAmount = (quantityQualification * storeItem.value) - amount;
                totalPrice -= item.discountAmount;

                // Display to customer
                specialOfferText = string.Format("-Applied Buy {0} for ${1} special for {2}!\n\t\t\t\t\t\t- ${3}", quantityQualification, amount, storeItem.name, item.discountAmount);
            }

            if (specialOfferText != "")
                Console.WriteLine(specialOfferText);

            return item;
        }

        private Item BuyNGetMForXSpecialWeightedItems(Item originalItem, Item discountedItem, float amount)
        {
            // Only apply to non-weighed items
            if (originalItem.type != Item.Type.Weighed)
                return discountedItem;

            //Only apply to items of lesser or equal value
            Item item = discountedItem;

            if (originalItem.value >= item.value)
            {
                item.discountAmount = item.price - (item.price * amount);
                totalPrice -= item.discountAmount;

                specialOfferText = string.Format("-Applied Buy {0} and get {1} of lesser or equal value for {2}% less!\n\t\t\t\t\t\t-${3}", originalItem.name, discountedItem.name, amount * 100f, item.discountAmount);
            }

            if (specialOfferText != "")
                Console.WriteLine(specialOfferText);

            return item;
        }

        // Remove Item Test
        private void RemoveStoreItem(Item storeItem, int count)
        {
            items.Remove(storeItem);
            storeItem.quanity--;
            string removeItemText = string.Format("\n\n{0} {1}(s) has been removed:", count, storeItem.name);

            float updatedPrice = ((storeItem.value * count) - storeItem.markDownAmount) + storeItem.discountAmount;

            //Update total price
            totalPrice -= updatedPrice;

            Console.WriteLine(removeItemText);
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("\n\t\t\t\t\t Total: $" + totalPrice);

        }

        // Display 
        private void DisplayCheckout(Item item)
        {
            if (item.type == Item.Type.Each)
            {
                if (item.quanity > 1)
                    Console.WriteLine("- " + item.name + "\t\t" + item.quanity + "X" + " ($" + item.value + " each) " + "\n\t\t\t\t\t\t$" + item.price);
                else
                    Console.WriteLine("- " + item.name + "\t\t\t\t\t\t$" + item.price);
            }
            else
            {
                Console.WriteLine("- " + item.name + "\t" + item.quanity + " lbs." + " ($" + item.value + " / lb.) " + "\n\t\t\t\t\t\t$" + item.price);
            }

            if (item.markDownAmount > 0.0f)
                Console.WriteLine(markedDownText);

            specialOfferText = "";

            Console.WriteLine("------------------------------------------------------");

        }

        private Item ScanItemInterface(Item storeItem)
        {
            if (storeItem.type != Item.Type.Weighed)
            {
                Console.WriteLine("How many would you like to scan?");

                int input;
                if (int.TryParse(Console.ReadLine(), out input))
                {
                    storeItem.quanity = input;
                }
                else
                {
                    Console.WriteLine("How many would you like to scan?");
                }
            }

            Item item = ScanItem(storeItem);

            DisplayCheckout(item);

            if (item.name == "Soup")
            {
                BuyNItemsGetMOffSpecial(item, 1, 0.5f, 4);
            }
            else if (item.name == "Milk")
            {
                if (item.quanity >= 3)
                    BuyNGetXSpecial(item, 3, 2.0f);
            }

            Console.WriteLine("\n\t\t\t\t\t Total: $" + totalPrice);

            return item;
        }
    }
}
