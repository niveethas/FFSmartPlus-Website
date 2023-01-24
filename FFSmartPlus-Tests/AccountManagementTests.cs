using ClientAPI;
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

        string trueStr = "True";
        string falseStr = "False";
        string trueSelfStr = "TrueSelf";

        string[] roles = { "Admin", "Chef", "Driver"};

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
            accountManagement._client.AddAuth(loginCode.Result.Token);

        }

        [TestMethod]
        public async Task T1_GetUsers_Valid()
        {
            await accountManagement.getUsers();

            Assert.IsTrue(accountManagement.users.Count > 0);
            validUsername = accountManagement.users.First();
        }


        [TestMethod]
        public async Task T2_addRole_Valid_Admin()
        {
            string role = roles[0];
            await accountManagement.addRole(validUsername, role);

            Assert.AreEqual(trueStr, accountManagement.success);
        }


        [TestMethod]
        public async Task T3_deleteRole_Admin()
        {
            string role = roles[0];
            await accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(trueStr, accountManagement.success);
        }

        [TestMethod]
        public async Task T4_addRole_Valid_Chef()
        {
            string role = roles[1];
            await accountManagement.addRole(validUsername, role);

            Assert.AreEqual(trueStr, accountManagement.success);
        }

        [TestMethod]
        public async Task T5_deleteRole_Admin()
        {
            string role = roles[1];
            await accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(trueStr, accountManagement.success);
        }

        [TestMethod]
        public async Task T6_addRole_Valid_Delivery()
        {
            string role = roles[2];
            await accountManagement.addRole(validUsername, role);

            Assert.AreEqual(trueStr, accountManagement.success);
        }

        [TestMethod]
        public async Task T7_deleteRole_Admin()
        {
            string role = roles[2];
            await accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(trueStr, accountManagement.success);
        }



    }
}
