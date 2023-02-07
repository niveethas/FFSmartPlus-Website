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

        private string userUsername = "userTest";
        private string userPassword = "@User123";

        private string invalidUsername = "qwerty";
        private string invalidPassword = "asdfgh";

        private string blankString = "";


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
            // login to valid admin user 
            await _login.userLogin(adminUsername, adminPassword);

            // check role was added sucessfully
            CollectionAssert.Contains(CurrentUserRoles._role, "Admin");

            Assert.AreEqual(TestConsts.TRUE_STR, _login.loginSuccess);
            _login.ToasterStatus();
        }

        [TestMethod]
        public async Task TA2_Login_ValidUser_User()
        {
            // login - valid standard user
            await _login.userLogin(userUsername, userPassword);

            Assert.IsNotNull(_login.loginResponse);
            // check role is read correctly
            CollectionAssert.DoesNotContain(CurrentUserRoles._role, "Admin"); 

            Assert.AreEqual(TestConsts.TRUE_STR, _login.loginSuccess);
            _login.ToasterStatus();
        }

        [TestMethod]
        public async Task TA3_Login_InvalidUsername_ValidPassword()
        {
            // login - invalid username but valid password combo
            await _login.userLogin(invalidUsername, adminPassword);

            Assert.AreEqual(TestConsts.FALSE_STR, _login.loginSuccess);
            _login.ToasterStatus();
        }

        [TestMethod]
        public async Task TA4_Login_BlankUsername_ValidPassword()
        {
            // login - empty username and valid password combo
            await _login.userLogin(blankString, adminPassword);

            Assert.AreEqual(TestConsts.FALSE_STR, _login.loginSuccess);
            _login.ToasterStatus();
        }

        [TestMethod]
        public async Task TA5_Login_InvalidPassword_ValidUsername()
        {
            // login - invalid password and valid username combo
            await _login.userLogin(adminUsername, invalidPassword);

            Assert.AreEqual(TestConsts.FALSE_STR, _login.loginSuccess);
            _login.ToasterStatus();
        }

        [TestMethod]
        public async Task TA6_Login_BlankPassword_ValidUsername()
        {
            // login - blank password and valid username combo
            await _login.userLogin(adminUsername, blankString);

            Assert.AreEqual(TestConsts.FALSE_STR, _login.loginSuccess);
            _login.ToasterStatus();
        }
    }
}
