using NUnit.Framework;

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
            testItem.quanity = 1;
            testItem.value = 2.50f;
            testItem.price = 0.0f;
            testItem.type = ItemManager.Item.Type.Each;
            testItem.markDownAmount = 0.0f;
            testItem.discountAmount = 0.0f;

            // Resulted item is the updated version of the testItem started with
            ItemManager.Item resultedItem;
            resultedItem.name = "Milk";
            resultedItem.quanity = 1;
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
            testItem.quanity = 10;
            testItem.value = 2.70f;
            testItem.price = 0.0f;
            testItem.type = ItemManager.Item.Type.Each;
            testItem.markDownAmount = 0.0f;
            testItem.discountAmount = 0.0f;

            float desiredMarkDownAmount = 1.0f;

            // Resulted item is the updated version of the testItem started with
            ItemManager.Item resultedItem;
            resultedItem.name = "Soup";
            resultedItem.quanity = 10;
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
            testItem.quanity = 6;
            testItem.value = 2.50f;
            testItem.price = 0.0f;
            testItem.type = ItemManager.Item.Type.Each;
            testItem.markDownAmount = 0.0f;
            testItem.discountAmount = 0.0f;


            // This item has coupon applied 
            ItemManager.Item resultItem;
            resultItem.name = "Milk";
            resultItem.quanity = 1;
            resultItem.value = 2.50f;
            resultItem.price = 0.00f; 
            resultItem.type = ItemManager.Item.Type.Each;
            resultItem.markDownAmount = 0.0f;
            resultItem.discountAmount = 2.50f; // Can accumulate up until specified max

            // Buy 1 get 1 50% off (Bought 4 and total discount is -$2.50) 
            // Can't reduce more than twice (Max reduction: $2.50)
            Assert.AreEqual(resultItem.discountAmount, itemManager.BuyNItemsGetMOffSpecial(testItem, 1, 0.5f, 2).discountAmount);
        }

    }
}
