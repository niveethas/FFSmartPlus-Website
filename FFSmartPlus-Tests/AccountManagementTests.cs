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
        AccountManagement accountManagement;

        string trueSelfStr = "TrueSelf";

        string[] roles = { "admin", "chef", "delivery"};

        string validUsername = "User2";

        public AccountManagementTests()
        {
            accountManagement = new AccountManagement();
            accountManagement._client = new FFSBackEnd("https://localhost:7041/", new HttpClient());

            var newLogin = new LoginModel();
            newLogin.Username = "admin1";
            newLogin.Password = "@Admin123"; // Password requires a symbol, capital letter and 3 numbers
            accountManagement._client.LoginAsync(newLogin);
            var loginCode = accountManagement._client.LoginAsync(newLogin);
            accountManagement._client.Authorisation(loginCode.Result.Token);

            CurrentUserRoles currentUser = new CurrentUserRoles() { Role = new List<string> { "admin", "chef" }, User = "admin1" };
            accountManagement.currentUser = currentUser;

        }


        [TestMethod]
        public async Task T0_GetUsers_Valid()
        {
            await accountManagement.getUsers();

            Assert.IsTrue(accountManagement.users.Count > 0);
            validUsername = accountManagement.users.First();
        }

        #region Admin

        [TestMethod]
        public async Task TA1_addRole_Valid_Admin()
        {
            string role = roles[0];
            await accountManagement.addRole(validUsername, role);

            Assert.AreEqual(TestConsts.TRUE_STR, accountManagement.success);
        }

        [TestMethod]
        public async Task TA2_addRole_Invalid_AlreadyHave_Admin()
        {
            string role = roles[0];
            await accountManagement.addRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, accountManagement.success);
        }


        [TestMethod]
        public async Task TA3_deleteRole_Valid_Admin()
        {
            string role = roles[0];
            await accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(TestConsts.TRUE_STR, accountManagement.success);
        }

        [TestMethod]
        public async Task TA4_deleteRole_Invalid_DontHave_Admin()
        {
            string role = roles[0];
            await accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, accountManagement.success);
        }

        #endregion

        #region Chef

        [TestMethod]
        public async Task TB1_addRole_Valid_Chef()
        {
            string role = roles[1];
            await accountManagement.addRole(validUsername, role);

            Assert.AreEqual(TestConsts.TRUE_STR, accountManagement.success);
        }

        [TestMethod]
        public async Task TB2_addRole_Invalid_AlreadyHave_Chef()
        {
            string role = roles[1];
            await accountManagement.addRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, accountManagement.success);
        }

        [TestMethod]
        public async Task TB3_deleteRole_Valid_Chef()
        {
            string role = roles[1];
            await accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(TestConsts.TRUE_STR, accountManagement.success);
        }

        [TestMethod]
        public async Task TB4_deleteRole_Invalid_DontHave_Chef()
        {
            string role = roles[1];
            await accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, accountManagement.success);
        }

        #endregion


        #region Delivery

        [TestMethod]
        public async Task TC1_addRole_Valid_Delivery()
        {
            string role = roles[2];
            await accountManagement.addRole(validUsername, role);

            Assert.AreEqual(TestConsts.TRUE_STR, accountManagement.success);
        }

        [TestMethod]
        public async Task TC2_addRole_Invalid_AlreadyHave_Delivery()
        {
            string role = roles[2];
            await accountManagement.addRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, accountManagement.success);
        }

        [TestMethod]
        public async Task TC3_deleteRole_Valid_Delivery()
        {
            string role = roles[2];
            await accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(TestConsts.TRUE_STR, accountManagement.success);
        }

        [TestMethod]
        public async Task TC4_deleteRole_Invalid_DontHave_Delivery()
        {
            string role = roles[2];
            await accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, accountManagement.success);
        }

        #endregion

        #region error tests

        [TestMethod]
        public async Task TD1_addRole_Invalid_Role()
        {
            string role = "adghf";
            await accountManagement.addRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, accountManagement.success);
        }

        [TestMethod]
        public async Task TD2_addRole_Invalid_User()
        {
            string role = roles[2];
            string username = "askmlfkl";
            await accountManagement.addRole(username, role);

            Assert.AreEqual(TestConsts.FALSE_STR, accountManagement.success);
        }

        [TestMethod]
        public async Task TD3_deleteRole_Invalid_Role()
        {
            string role = "adghf";
            await accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, accountManagement.success);
        }

        [TestMethod]
        public async Task TD4_deleteRole_Invalid_User()
        {
            string role = roles[2];
            string username = "askjakgkl";
            await accountManagement.deleteRole(username, role);

            Assert.AreEqual(TestConsts.FALSE_STR, accountManagement.success);
        }
        #endregion


    }
}
