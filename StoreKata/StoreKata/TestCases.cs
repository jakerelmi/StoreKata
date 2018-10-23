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
        
    }
}
