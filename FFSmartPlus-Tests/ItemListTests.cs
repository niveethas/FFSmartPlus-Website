using ClientAPI;
using FFSmartPlus_Website.Pages;

namespace FFSmartPlus_Tests
{
    [TestClass]
    public class ItemListTests
    {
        private readonly ItemList itemList;
        int validItemId = 3;
        private string[] validNewItem = { "Apple", "per Bag", "1", "1", "2" };
                                        //name, unitDesc, minStock, supplierID, desiredStock
        private long newItemId = 2;

        private int stockCount = 0;

        public ItemListTests()
        {
            itemList = new ItemList();
            itemList._client = new FFSBackEnd("https://localhost:7041/", new HttpClient());
            
            var newLogin = new LoginModel();
            newLogin.Username = "admin1";
            newLogin.Password = "@Admin123";
            var loginCode =  itemList._client.LoginAsync(newLogin);
            itemList._client.AddAuth(loginCode.Result.Token);

        }

        #region LoadItem Tests
        // --- loadItem(string itemID) Tests ---

        // Testing load items for a valid item id that exists in the database
        [TestMethod]
        public async Task TA1_LoadItem_ValidId()
        {
            await itemList.loadItem(validItemId.ToString());
            Assert.AreEqual(validItemId, itemList.itemInfo.Id);
        }

        // Testing load items for a valid item id that doesn't exist in the database
        [TestMethod]
        public async Task TA2_LoadItem_NonExistentItem()
        {
            await itemList.loadItem("0");
            Assert.IsNull(itemList.itemInfo);
        }

        // Testing load items for a invalid item id - string character
        [TestMethod]
        public async Task TA3_LoadItem_InvalidId_StringChar()
        {
            await itemList.loadItem("A");
            Assert.IsNull(itemList.itemInfo);
        }

        // Testing load items for a invalid item id - special character
        [TestMethod]
        public async Task TA4_LoadItem_InvalidId_SpecialChar()
        {
            await itemList.loadItem("!");
            Assert.IsNull(itemList.itemInfo);
        }
        #endregion


        #region addNewItem Tests
        // ---addNewItem(string name, string unitDesc, string minStock, string supplierID, string desiredStock ) Tests ---
        [TestMethod]
        public async Task TB1_addNewItem_Valid()
        {
            await itemList.addNewItem(validNewItem[0], validNewItem[1], validNewItem[2], validNewItem[3], validNewItem[4]);
            Assert.IsTrue(itemList.itemAdditionSuccess);
            newItemId = itemList.returnedId;
        }

        [TestMethod]
        public async Task TB2_addNewItem_Valid_DecimalMaxStockAndDesiredStock()
        {
            string decimalStr = "0.1";
            await itemList.addNewItem(validNewItem[0], validNewItem[1], decimalStr, validNewItem[3], decimalStr);
            Assert.IsTrue(itemList.itemAdditionSuccess);
        }

        [TestMethod]
        public async Task TB3_addNewItem_InvalidSupplierID()
        {
            string invalidId = "500";
            await itemList.addNewItem(validNewItem[0], validNewItem[1], validNewItem[2], invalidId, validNewItem[4]);
            Assert.IsFalse(itemList.itemAdditionSuccess);
        }

        [TestMethod]
        public async Task TB4_addNewItem_InvalidMaxStock_AlphaChars()
        {
            string invalidMaxStock = "abc";
            await itemList.addNewItem(validNewItem[0], validNewItem[1], invalidMaxStock, validNewItem[3], validNewItem[4]);
            Assert.IsFalse(itemList.itemAdditionSuccess);
        }


        [TestMethod]
        public async Task TB5_addNewItem_InvalidDesiredStock_AlphaChars()
        {
            string invalidDesiredStock = "abc";
            await itemList.addNewItem(validNewItem[0], validNewItem[1], validNewItem[2], validNewItem[3], invalidDesiredStock);
            Assert.IsFalse(itemList.itemAdditionSuccess);
        }

        #endregion

        #region addStock Tests
        // --- addStock (string quantity, DateTime expiryDate) ---
        [TestMethod]
        public async Task TC1_addStock_Valid()
        {
            itemList.returnedId = validItemId;
            String quantity = "5";
            DateTime expiry = DateTime.Now.AddMonths(2);
            await itemList.addStock(quantity, expiry);

            await itemList.loadItem(validItemId.ToString());
            Assert.IsTrue(itemList.unitStock.Last().Quantity == Convert.ToDouble(quantity) 
                && itemList.unitStock.Last().ExpiryDate.Date == expiry.Date);
        }

        [TestMethod]
        public async Task TC2_addStock_Valid_DecimalQuantity()
        {
            itemList.returnedId = validItemId;
            String quantity = "0.5";
            DateTime expiry = DateTime.Now.AddMonths(2);
            await itemList.addStock(quantity, expiry);


            await itemList.loadItem(validItemId.ToString());
            stockCount = itemList.unitStock.Count();

            Assert.IsTrue(itemList.unitStock.Last().Quantity == Convert.ToDouble(quantity)
                && itemList.unitStock.Last().ExpiryDate.Date == expiry.Date);

        }

        [TestMethod]
        public async Task TC3_addStock_Invalid_NonNumericQuantity()
        {
            itemList.returnedId = validItemId;
            String quantity = "ABC";
            DateTime expiry = DateTime.Now.AddMonths(2);
            await itemList.addStock(quantity, expiry);

            await itemList.loadItem(validItemId.ToString());
            Assert.IsTrue(itemList.unitStock.Count() == stockCount);
        }

        #endregion

        #region deleteItem Tests
        // --- deleteItem(long id) ---

        [TestMethod]
        public async Task TD1_deleteItem_ValidId()
        {
            await itemList.deleteItem(newItemId);
            Assert.IsNull(itemList.itemInfo);
        }

        [TestMethod]
        public async Task TD2_deleteItem2_InvalidId()
        {
            long invalidId = 5000;
            await itemList.deleteItem(invalidId);
            Assert.IsNull(itemList.itemInfo);
        }
        #endregion
    }

}