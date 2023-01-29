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
        Login _login;

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
            _login = new Login();
            _login._client = new FFSBackEnd("https://localhost:7041/", new HttpClient());

            _login.NavManager = new TestNav();
        }

        // --- userLogin(string username, string password) --- 

        [TestMethod]
        public async Task TA1_Login_ValidUser_Admin()
        {
            await _login.userLogin(adminUsername, adminPassword);
            CollectionAssert.Contains(CurrentUserRoles._role, "Admin");

            Assert.AreEqual(TestConsts.TRUE_STR, _login.loginSuccess);
            _login.ToasterStatus();
        }

        [TestMethod]
        public async Task TA2_Login_ValidUser_User()
        {
            await _login.userLogin(userUsername, userPassword);

            Assert.IsNotNull(_login.loginResponse);
            CollectionAssert.DoesNotContain(CurrentUserRoles._role, "Admin");

            Assert.AreEqual(TestConsts.TRUE_STR, _login.loginSuccess);
            _login.ToasterStatus();
        }

        [TestMethod]
        public async Task TA3_Login_InvalidUsername_ValidPassword()
        {
            await _login.userLogin(invalidUsername, adminPassword);

            Assert.AreEqual(TestConsts.FALSE_STR, _login.loginSuccess);
            _login.ToasterStatus();
        }

        [TestMethod]
        public async Task TA4_Login_BlankUsername_ValidPassword()
        {
            await _login.userLogin(blankString, adminPassword);

            Assert.AreEqual(TestConsts.FALSE_STR, _login.loginSuccess);
            _login.ToasterStatus();
        }

        [TestMethod]
        public async Task TA5_Login_InvalidPassword_ValidUsername()
        {
            await _login.userLogin(adminUsername, invalidPassword);

            Assert.AreEqual(TestConsts.FALSE_STR, _login.loginSuccess);
            _login.ToasterStatus();
        }

        [TestMethod]
        public async Task TA6_Login_BlankPassword_ValidUsername()
        {
            await _login.userLogin(adminUsername, blankString);

            Assert.AreEqual(TestConsts.FALSE_STR, _login.loginSuccess);
            _login.ToasterStatus();
        }
    }
}
