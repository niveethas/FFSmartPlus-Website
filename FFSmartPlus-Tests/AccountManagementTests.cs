using ClientAPI;
using FFSmartPlus_Website;
using FFSmartPlus_Website.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFSmartPlus_Tests
{

    [TestClass]
    public class AccountManagementTests
    {
        AccountManagement _accountManagement;

        string[] roles = { "admin", "chef", "delivery"};

        string validUsername = "User2";

        public AccountManagementTests()
        {
            _accountManagement = new AccountManagement();
            _accountManagement._client = new SetupClient().SignInAdmin();

            CurrentUserRoles currentUser = new CurrentUserRoles() { Role = new List<string> { "admin", "chef" }, User = "admin1" };
            _accountManagement.currentUser = currentUser;

        }


        [TestMethod]
        public async Task T0_GetUsers_Valid()
        {
            await _accountManagement.getUsers();

            Assert.IsTrue(_accountManagement.users.Count > 0);
            validUsername = _accountManagement.users.First();
        }

        #region Admin

        [TestMethod]
        public async Task TA1_addRole_Valid_Admin()
        {
            string role = roles[0];
            await _accountManagement.addRole(validUsername, role);

            Assert.AreEqual(TestConsts.TRUE_STR, _accountManagement.success);
        }

        [TestMethod]
        public async Task TA2_addRole_Invalid_AlreadyHave_Admin()
        {
            string role = roles[0];
            await _accountManagement.addRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, _accountManagement.success);
        }


        [TestMethod]
        public async Task TA3_deleteRole_Valid_Admin()
        {
            string role = roles[0];
            await _accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(TestConsts.TRUE_STR, _accountManagement.success);
        }

        [TestMethod]
        public async Task TA4_deleteRole_Invalid_DontHave_Admin()
        {
            string role = roles[0];
            await _accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, _accountManagement.success);
        }

        #endregion

        #region Chef

        [TestMethod]
        public async Task TB1_addRole_Valid_Chef()
        {
            string role = roles[1];
            await _accountManagement.addRole(validUsername, role);

            Assert.AreEqual(TestConsts.TRUE_STR, _accountManagement.success);
        }

        [TestMethod]
        public async Task TB2_addRole_Invalid_AlreadyHave_Chef()
        {
            string role = roles[1];
            await _accountManagement.addRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, _accountManagement.success);
        }

        [TestMethod]
        public async Task TB3_deleteRole_Valid_Chef()
        {
            string role = roles[1];
            await _accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(TestConsts.TRUE_STR, _accountManagement.success);
        }

        [TestMethod]
        public async Task TB4_deleteRole_Invalid_DontHave_Chef()
        {
            string role = roles[1];
            await _accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, _accountManagement.success);
        }

        #endregion


        #region Delivery

        [TestMethod]
        public async Task TC1_addRole_Valid_Delivery()
        {
            string role = roles[2];
            await _accountManagement.addRole(validUsername, role);

            Assert.AreEqual(TestConsts.TRUE_STR, _accountManagement.success);
        }

        [TestMethod]
        public async Task TC2_addRole_Invalid_AlreadyHave_Delivery()
        {
            string role = roles[2];
            await _accountManagement.addRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, _accountManagement.success);
        }

        [TestMethod]
        public async Task TC3_deleteRole_Valid_Delivery()
        {
            string role = roles[2];
            await _accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(TestConsts.TRUE_STR, _accountManagement.success);
        }

        [TestMethod]
        public async Task TC4_deleteRole_Invalid_DontHave_Delivery()
        {
            string role = roles[2];
            await _accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, _accountManagement.success);
        }

        #endregion

        #region error tests

        [TestMethod]
        public async Task TD1_addRole_Invalid_Role()
        {
            string role = "adghf";
            await _accountManagement.addRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, _accountManagement.success);
        }

        [TestMethod]
        public async Task TD2_addRole_Invalid_User()
        {
            string role = roles[2];
            string username = "askmlfkl";
            await _accountManagement.addRole(username, role);

            Assert.AreEqual(TestConsts.FALSE_STR, _accountManagement.success);
        }

        [TestMethod]
        public async Task TD3_deleteRole_Invalid_Role()
        {
            string role = "adghf";
            await _accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, _accountManagement.success);
        }

        [TestMethod]
        public async Task TD4_deleteRole_Invalid_User()
        {
            string role = roles[2];
            string username = "askjakgkl";
            await _accountManagement.deleteRole(username, role);

            Assert.AreEqual(TestConsts.FALSE_STR, _accountManagement.success);
        }
        #endregion


    }
}
