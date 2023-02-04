using ClientAPI;
using FFSmartPlus_Website.Pages;
using FFSmartPlus_Website;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FFSmartPlus_Tests
{
    [TestClass]
    public class AdminAuditTests
    {
        AdminAudit _adminAudit;

        List<UnitListDto> expStockList;

        public AdminAuditTests()
        {
            _adminAudit = new AdminAudit();
            _adminAudit._client = new SetupClient().SignInAdmin();
        }

        [TestMethod]
        public async Task TA1_expStockToList()
        {
            // testing expiring stock list function 

            // adding an expired item to the stock to guarantee there is a value
            var newAUD = new NewUnitDto();
            newAUD.ExpiryDate = DateTime.Now.AddDays(-7);
            newAUD.Quantity = 5;
            long validId = 1;
            await _adminAudit._client.AddAsync(validId, newAUD);

            // populating expired stock - usually done at page initiation 
            _adminAudit.allExpStock = await _adminAudit._client.ExpiryAsync();

            // saving returned list - for this tc and later ones
            expStockList = _adminAudit.expStockToList();

            Assert.IsTrue(expStockList.Count() > 0); 
        }

        [TestMethod]
        public async Task TA2_deleteExpStock()
        {
            // delete the expired stock
            await _adminAudit.deleteExpStock();

            Assert.AreEqual(TestConsts.TRUE_STR, _adminAudit.deletionSuccess);
        }

        [TestMethod]
        public async Task TA3_expStockToList_NoneExpired()
        {
            // getting stock list when there is no expired stock - from last tc
            // refreshing stock list
            _adminAudit.allExpStock = await _adminAudit._client.ExpiryAsync();
            List<UnitListDto> newList = _adminAudit.expStockToList();

            CollectionAssert.AreNotEqual(expStockList, newList);
        }

        [TestMethod]
        public async Task TA4_deleteExpStock_NoneExpired()
        {
            // delete expired stock when there is non expired - from previous tc
            await _adminAudit.deleteExpStock();

            Assert.AreEqual(TestConsts.TRUE_STR, _adminAudit.deletionSuccess);
        }


    }
}
