using NUnit.Framework;
using System;

namespace StoreKata
{
    [TestFixture]
    class TestCases
    {
        private ItemManager itemManager = new ItemManager();

        [TestCase]
        public void ScanItemTest()
        {
            // Original Item Properties
            ItemManager.Item testItem;
            testItem.name = "Milk";
            testItem.quantity = 1;
            testItem.value = 2.50f;
            testItem.price = 0.0f;
            testItem.type = ItemManager.Item.Type.Each;
            testItem.markDownAmount = 0.0f;
            testItem.discountAmount = 0.0f;

            // Resulted item is the updated version of the testItem started with
            ItemManager.Item resultedItem;
            resultedItem.name = "Milk";
            resultedItem.quantity = 1;
            resultedItem.value = 2.50f;
            resultedItem.price = 2.50f; // Price gets updated from 0.0f
            resultedItem.type = ItemManager.Item.Type.Each;
            resultedItem.markDownAmount = 0.0f;
            resultedItem.discountAmount = 0.0f;

            Assert.AreEqual(resultedItem, itemManager.ScanItem(testItem));

        }

        [TestCase]
        public void MarkDownItemTest()
        {
            ItemManager.Item testItem;
            testItem.name = "Soup";
            testItem.quantity = 10;
            testItem.value = 2.70f;
            testItem.price = 0.0f;
            testItem.type = ItemManager.Item.Type.Each;
            testItem.markDownAmount = 0.0f;
            testItem.discountAmount = 0.0f;

            float desiredMarkDownAmount = 1.0f;

            // Resulted item is the updated version of the testItem started with
            ItemManager.Item resultedItem;
            resultedItem.name = "Soup";
            resultedItem.quantity = 10;
            resultedItem.value = 1.70f;
            resultedItem.price = 0.0f;
            resultedItem.type = ItemManager.Item.Type.Each;
            resultedItem.markDownAmount = 0.0f;
            resultedItem.discountAmount = 0.0f;

            Assert.AreEqual(resultedItem, itemManager.Markdown(testItem, desiredMarkDownAmount));
        }

        [TestCase]
        public void BuyNItemsGetMOffSpecialTest()
        {
            ItemManager.Item testItem;

            testItem.name = "Milk";
            testItem.quantity = 6;
            testItem.value = 2.50f;
            testItem.price = 0.0f;
            testItem.type = ItemManager.Item.Type.Each;
            testItem.markDownAmount = 0.0f;
            testItem.discountAmount = 0.0f;


            // This item has coupon applied 
            ItemManager.Item resultItem;
            resultItem.name = "Milk";
            resultItem.quantity = 1;
            resultItem.value = 2.50f;
            resultItem.price = 0.00f; 
            resultItem.type = ItemManager.Item.Type.Each;
            resultItem.markDownAmount = 0.0f;
            resultItem.discountAmount = 2.50f; // Can accumulate up until specified max

            // Buy 1 get 1 50% off (Bought 4 and total discount is -$2.50) 
            // Can't reduce more than twice (Max reduction: $2.50)
            Assert.AreEqual(resultItem.discountAmount, itemManager.BuyNItemsGetMOffSpecial(testItem, 1, 0.5f, 2).discountAmount);
        }

        [TestCase]
        public void BuyNGetXSpecialTest()
        {
            ItemManager.Item testItem;
            testItem.name = "Soup";
            testItem.quantity = 3;
            testItem.value = 2.70f;
            testItem.price = 0.0f;
            testItem.type = ItemManager.Item.Type.Each;
            testItem.markDownAmount = 0.0f;
            testItem.discountAmount = 0.0f;

            ItemManager.Item resultItem;
            resultItem.name = "Soup";
            resultItem.quantity = 1;
            resultItem.value = 2.70f;
            resultItem.price = 0.0f;
            resultItem.type = ItemManager.Item.Type.Each;
            resultItem.markDownAmount = 0.0f;
            resultItem.discountAmount = 6.10f; 

            // Buy 3 for $2.00 soup (Normal price = $8.10, discount amount after sale = $6.10)
            Assert.AreEqual(resultItem.discountAmount,2, Math.Round(itemManager.BuyNGetXSpecial(testItem, 3, 2.0f).discountAmount, 2));

        }

        [TestCase]
        public void BuyNGetMForXSpecialWeightedItemsTest()
        {
            ItemManager.Item testItem1;
            testItem1.name = "Bananas";
            testItem1.quantity = 0.4f;
            testItem1.value = 1.89f;
            testItem1.price = 0.0f;
            testItem1.type = ItemManager.Item.Type.Weighed;
            testItem1.markDownAmount = 0.0f;
            testItem1.discountAmount = 0.0f;
            
            // Item that gets offer applied to (Must be <= value)
            ItemManager.Item testItem2;
            testItem2.name = "Beef";
            testItem2.quantity = 1.0f;
            testItem2.value = 1.70f;
            testItem2.price = 0.0f;
            testItem2.type = ItemManager.Item.Type.Weighed;
            testItem2.markDownAmount = 0.0f;
            testItem2.discountAmount = 0.0f;

            // Must scan items first before applying special offer
            testItem1 = itemManager.ScanItem(testItem1);
            testItem2 = itemManager.ScanItem(testItem2);

            float resultedDiscountAmount = 0.85f; // Second item is half off ($1.70 * 0.5)

            Assert.AreEqual(resultedDiscountAmount, itemManager.BuyNGetMForXSpecialWeightedItems(testItem1, testItem2, 0.5f).discountAmount);
        }

        [TestCase]
        public void RemoveStoreItemTest()
        {
            // Run this test separately (total price affected by previous tests)
            ItemManager.Item testItem;
            testItem.name = "Soup";
            testItem.quantity = 10;
            testItem.value = 2.70f;
            testItem.price = 0.0f;
            testItem.type = ItemManager.Item.Type.Each;
            testItem.markDownAmount = 0.0f;
            testItem.discountAmount = 0.0f;

            // Remove can only happen after a scan 
            testItem = itemManager.ScanItem(testItem);

            //Should update the overall price 
            float resultedPrice = 24.3f; 

            itemManager.RemoveStoreItem(testItem, 1);
            Assert.AreEqual(resultedPrice, itemManager.totalPrice);
        }
    }
}
