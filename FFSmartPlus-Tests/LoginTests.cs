using ClientAPI;
using FFSmartPlus_Website;
using FFSmartPlus_Website.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFSmartPlus_Tests
{
    [TestClass]
    public class LoginTests
    {
        Login login;

        private String adminUsername = "admin1";
        private String adminPassword = "@Admin123";

        private String userUsername = "userTest";
        private String userPassword = "@User123";

        private String invalidUsername = "qwerty";
        private String invalidPassword = "asdfgh";

        private String blankString = "";

        //private string trueStr = "true";
        //private string falseStr = "false";

        public LoginTests()
        {
            login = new Login();
            login._client = new FFSBackEnd("https://localhost:7041/", new HttpClient());

            login.NavManager = new TestNav();
        }

        // --- userLogin(string username, string password) --- 

        [TestMethod]
        public async Task TA1_Login_ValidUser_Admin()
        {
            await login.userLogin(adminUsername, adminPassword);
            CollectionAssert.Contains(CurrentUserRoles._role, "Admin");

            Assert.AreEqual(TestConsts.TRUE_STR, login.loginSuccess);
            login.ToasterStatus();
        }

        [TestMethod]
        public async Task TA2_Login_ValidUser_User()
        {
            await login.userLogin(userUsername, userPassword);

            Assert.IsNotNull(login.loginResponse);
            CollectionAssert.DoesNotContain(CurrentUserRoles._role, "Admin");

            Assert.AreEqual(TestConsts.TRUE_STR, login.loginSuccess);
            login.ToasterStatus();
        }

        [TestMethod]
        public async Task TA3_Login_InvalidUsername_ValidPassword()
        {
            await login.userLogin(invalidUsername, adminPassword);

            Assert.AreEqual(TestConsts.FALSE_STR, login.loginSuccess);
            login.ToasterStatus();
        }

        [TestMethod]
        public async Task TA4_Login_BlankUsername_ValidPassword()
        {
            await login.userLogin(blankString, adminPassword);

            Assert.AreEqual(TestConsts.FALSE_STR, login.loginSuccess);
            login.ToasterStatus();
        }

        [TestMethod]
        public async Task TA5_Login_InvalidPassword_ValidUsername()
        {
            await login.userLogin(adminUsername, invalidPassword);

            Assert.AreEqual(TestConsts.FALSE_STR, login.loginSuccess);
            login.ToasterStatus();
        }

        [TestMethod]
        public async Task TA6_Login_BlankPassword_ValidUsername()
        {
            await login.userLogin(adminUsername, blankString);

            Assert.AreEqual(TestConsts.FALSE_STR, login.loginSuccess);
            login.ToasterStatus();
        }
    }
}
