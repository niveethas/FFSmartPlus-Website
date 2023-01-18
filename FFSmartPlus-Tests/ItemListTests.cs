using ClientAPI;
using FFSmartPlus_Website.Pages;

namespace FFSmartPlus_Tests
{
    [TestClass]
    public class ItemListTests
    {

        private readonly ItemList itemList;
        int validItemId = 1;
        private string[] validNewItem = { "Apple", "per Bag", "1", "1", "2" };
        //name, unitDesc, minStock, supplierID, desiredStock
        private long newItemId = 3;

        public ItemListTests()
        {
            itemList = new ItemList();
            itemList._client = new FFSBackEnd("https://localhost:7041/", new HttpClient());
        }

        // --- loadItem(string itemID) Tests ---
        #region LoadItem Tests

        // Testing load items for a valid item id that exists in the database
        [TestMethod]
        public async Task LoadItem_ValidId()
        {
            await itemList.loadItem(validItemId.ToString());
            Assert.AreEqual(validItemId, itemList.itemInfo.Id);
        }

        // Testing load items for a valid item id that doesn't exist in the database
        [TestMethod]
        public async Task LoadItem_NonExistentItem()
        {
            await itemList.loadItem("0");
            Assert.IsNull(itemList.itemInfo);
        }

        // Testing load items for a invalid item id - string character
        [TestMethod]
        public async Task LoadItem_InvalidId_StringChar()
        {
            await itemList.loadItem("A");
            Assert.IsNull(itemList.itemInfo);
        }

        // Testing load items for a invalid item id - special character
        [TestMethod]
        public async Task LoadItem_InvalidId_SpecialChar()
        {
            await itemList.loadItem("!");
            Assert.IsNull(itemList.itemInfo);
        }
        #endregion

        // ---addNewItem(string name, string unitDesc, string minStock, string supplierID, string desiredStock ) Tests ---
        [TestMethod]
        public async Task addNewItem_ValidId()
        {
            await itemList.addNewItem(validNewItem[0], validNewItem[1], validNewItem[2], validNewItem[3], validNewItem[4]);
            Assert.IsTrue(itemList.itemAdditionSuccess);
            newItemId = itemList.returnedId;
        }

        // --- deleteItem(long id) ---

        [TestMethod]
        public async Task deleteItem_ValidId()
        {
            await itemList.deleteItem(newItemId);
            Assert.AreEqual(validItemId, itemList.itemInfo.Id);
        }
    }

}