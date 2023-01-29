using ClientAPI;
using FFSmartPlus_Website.Pages;

namespace FFSmartPlus_Tests
{
    [TestClass]
    public class ItemListTests
    {
        private ItemList _itemList;
        int validItemId = 3;
        private string[] validNewItem = { "Apple", "per Bag", "1", "1", "2" };
                                        //name, unitDesc, minStock, supplierID, desiredStock
        private long newItemId = 2;

        private int stockCount;

        public ItemListTests()
        {
            _itemList = new ItemList();
            _itemList._client = new SetupClient().SignInAdmin();
        }

        #region LoadItem Tests
        // --- loadItem(string itemID) Tests ---

        // Testing load items for a valid item id that exists in the database
        [TestMethod]
        public async Task TA1_LoadItem_ValidId()
        {
            await _itemList.loadItem(validItemId.ToString());
            Assert.AreEqual(validItemId, _itemList.itemInfo.Id);
        }

        // Testing load items for a valid item id that doesn't exist in the database
        [TestMethod]
        public async Task TA2_LoadItem_NonExistentItem()
        {
            await _itemList.loadItem("0");
            Assert.IsNull(_itemList.itemInfo);
        }

        // Testing load items for a invalid item id - string character
        [TestMethod]
        public async Task TA3_LoadItem_InvalidId_StringChar()
        {
            await _itemList.loadItem("A");
            Assert.IsNull(_itemList.itemInfo);
        }

        // Testing load items for a invalid item id - special character
        [TestMethod]
        public async Task TA4_LoadItem_InvalidId_SpecialChar()
        {
            await _itemList.loadItem("!");
            Assert.IsNull(_itemList.itemInfo);
        }
        #endregion


        #region addNewItem Tests
        // ---addNewItem(string name, string unitDesc, string minStock, string supplierID, string desiredStock ) Tests ---
        [TestMethod]
        public async Task TB1_addNewItem_Valid()
        {
            await _itemList.addNewItem(validNewItem[0], validNewItem[1], validNewItem[2], validNewItem[3], validNewItem[4]);
            Assert.IsTrue(_itemList.itemAdditionSuccess);
            newItemId = _itemList.returnedId;
        }

        [TestMethod]
        public async Task TB2_addNewItem_Valid_DecimalMaxStockAndDesiredStock()
        {
            string decimalStr = "0.1";
            await _itemList.addNewItem(validNewItem[0], validNewItem[1], decimalStr, validNewItem[3], decimalStr);
            Assert.IsTrue(_itemList.itemAdditionSuccess);
        }

        [TestMethod]
        public async Task TB3_addNewItem_InvalidSupplierID()
        {
            string invalidId = "500";
            await _itemList.addNewItem(validNewItem[0], validNewItem[1], validNewItem[2], invalidId, validNewItem[4]);
            Assert.IsFalse(_itemList.itemAdditionSuccess);
        }

        [TestMethod]
        public async Task TB4_addNewItem_InvalidMaxStock_AlphaChars()
        {
            string invalidMaxStock = "abc";
            await _itemList.addNewItem(validNewItem[0], validNewItem[1], invalidMaxStock, validNewItem[3], validNewItem[4]);
            Assert.IsFalse(_itemList.itemAdditionSuccess);
        }


        [TestMethod]
        public async Task TB5_addNewItem_InvalidDesiredStock_AlphaChars()
        {
            string invalidDesiredStock = "abc";
            await _itemList.addNewItem(validNewItem[0], validNewItem[1], validNewItem[2], validNewItem[3], invalidDesiredStock);
            Assert.IsFalse(_itemList.itemAdditionSuccess);
        }

        #endregion

        #region addStock Tests
        // --- addStock (string quantity, DateTime expiryDate) ---
        [TestMethod]
        public async Task TC1_addStock_Valid()
        {
            _itemList.returnedId = validItemId;
            String quantity = "5";
            DateTime expiry = DateTime.Now.AddMonths(2);
            await _itemList.addStock(quantity, expiry);

            Assert.AreEqual(TestConsts.TRUE_STR ,_itemList.stockModifySuccess);

            await _itemList.loadStock(validItemId.ToString());
            Assert.IsTrue(_itemList.unitStock.Last().Quantity == Convert.ToDouble(quantity) 
                && _itemList.unitStock.Last().ExpiryDate.Date == expiry.Date);
        }

        [TestMethod]
        public async Task TC2_addStock_Valid_DecimalQuantity()
        {
            _itemList.returnedId = validItemId;
            String quantity = "0.5";
            DateTime expiry = DateTime.Now.AddMonths(2);
            await _itemList.addStock(quantity, expiry);

            Assert.AreEqual(TestConsts.TRUE_STR, _itemList.stockModifySuccess);

            await _itemList.loadStock(validItemId.ToString());
            UnitsDto lastUnit = _itemList.unitStock.Last();

            Assert.IsTrue(lastUnit.Quantity == Convert.ToDouble(quantity)
                && lastUnit.ExpiryDate.Date == expiry.Date);

        }

        [TestMethod]
        public async Task TC3_addStock_Invalid_NonNumericQuantity()
        {
            _itemList.returnedId = validItemId;
            String quantity = "ABC";
            DateTime expiry = DateTime.Now.AddMonths(2);

            await _itemList.loadStock(validItemId.ToString());
            stockCount = _itemList.unitStock.Count();


            await _itemList.addStock(quantity, expiry);

            Assert.AreEqual(TestConsts.FALSE_STR, _itemList.stockModifySuccess);
            
            await _itemList.loadItem(validItemId.ToString());
            Assert.AreEqual(stockCount, _itemList.unitStock.Count());
        }

        [TestMethod]
        public async Task TC4_addStock_Invalid_InvalidId()
        {
            _itemList.returnedId = 500;
            String quantity = "10";
            DateTime expiry = DateTime.Now.AddMonths(2);
            await _itemList.addStock(quantity, expiry);

            Assert.AreEqual(TestConsts.FALSE_STR, _itemList.stockModifySuccess);
        }

        #endregion

        #region deleteItem Tests
        // --- deleteItem(long id) ---

        [TestMethod]
        public async Task TD1_deleteItem_ValidId()
        {
            await _itemList.deleteItem(newItemId);
            Assert.IsNull(_itemList.itemInfo);
        }

        [TestMethod]
        public async Task TD2_deleteItem2_InvalidId()
        {
            long invalidId = 5000;
            await _itemList.deleteItem(invalidId);
            Assert.IsNull(_itemList.itemInfo);
        }
        #endregion
    }

}