using ClientAPI;
using FFSmartPlus_Website.Pages;

namespace FFSmartPlus_Tests
{
    [TestClass]
    public class ItemTests
    {

        private readonly ItemList itemList;
        int itemID = 1;

        public ItemTests()
        {
            itemList = new ItemList();
            itemList._client = new FFSBackEnd("https://localhost:7041/", new HttpClient());
        }

        [TestMethod]
        public async Task LoadItemAsync()
        {
            await itemList.loadItem(itemID.ToString());
            Assert.AreEqual(itemID, itemList.itemInfo.Id);
        }
    }
}