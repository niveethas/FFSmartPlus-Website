using ClientAPI;
using FFSmartPlus_Website.Pages;
using FFSmartPlus_Website;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _adminAudit._client = new FFSBackEnd("https://localhost:7041/", new HttpClient());

            var newLogin = new LoginModel();
            newLogin.Username = "Admin2";
            newLogin.Password = "@Admin123"; // Password requires a symbol, capital letter and 3 numbers
            _adminAudit._client.LoginAsync(newLogin);
            var loginCode = _adminAudit._client.LoginAsync(newLogin);
            _adminAudit._client.Authorisation(loginCode.Result.Token);
        }

        //expStockToList()
        [TestMethod]
        public async Task T1_expStockToList()
        {
            var newAUD = new NewUnitDto();
            newAUD.ExpiryDate = DateTime.Now.AddDays(-7);
            newAUD.Quantity = 5;

            long validId = 1;

            await _adminAudit._client.AddAsync(validId, newAUD);

            _adminAudit.allExpStock = await _adminAudit._client.ExpiryAsync();

            expStockList = _adminAudit.expStockToList();

            Assert.IsTrue(expStockList.Count() > 0);
        }

        [TestMethod]
        public async Task T2_deleteExpStock()
        {
            await _adminAudit.deleteExpStock();

            Assert.AreEqual(TestConsts.TRUE_STR, _adminAudit.deletionSuccess);
        }

        [TestMethod]
        public async Task T3_expStockToList_NoneExpired()
        {
            _adminAudit.allExpStock = await _adminAudit._client.ExpiryAsync();
            List<UnitListDto> newList = _adminAudit.expStockToList();

            CollectionAssert.AreNotEqual(expStockList, newList);
        }

        [TestMethod]
        public async Task T4_deleteExpStock_NoneExpired()
        {
            await _adminAudit.deleteExpStock();

            Assert.AreEqual(TestConsts.TRUE_STR, _adminAudit.deletionSuccess);
        }

        [TestMethod]
        public async Task T5_getAuditHistory_Valid()
        {
            string days = "3";
            await _adminAudit.getAuditHistory(days);

            Assert.AreEqual(TestConsts.TRUE_STR, _adminAudit.auditSuccess);
        }

        [TestMethod]
        public async Task T6_getAuditHistory_Invalid_StrCha()
        {
            string daysInv = "ABC";
            await _adminAudit.getAuditHistory(daysInv);

            Assert.AreEqual(TestConsts.FALSE_STR, _adminAudit.auditSuccess);
        }

    }
}
