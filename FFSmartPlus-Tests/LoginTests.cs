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

        public LoginTests()
        {
            login = new Login();
            login._client = new FFSBackEnd("https://localhost:7041/", new HttpClient());

            login.NavManager = new TestNav();
        }

        // --- userLogin(string username, string password) --- 

        [TestMethod]
        public async Task Login1_ValidUser_Admin()
        {
            await login.userLogin(adminUsername, adminPassword);
            CollectionAssert.Contains(CurrentUserRoles._role, "Admin");

        }

        [TestMethod]
        public async Task Login2_ValidUser_User()
        {
            await login.userLogin(userUsername, userPassword);

            Assert.IsNotNull(login.loginResponse);
            CollectionAssert.DoesNotContain(CurrentUserRoles._role, "Admin");
        }

        [TestMethod]
        public async Task Login3_InvalidUsername_ValidPassword()
        {
            await login.userLogin(invalidUsername, adminPassword);
        }

        [TestMethod]
        public async Task Login4_BlankUsername_ValidPassword()
        {
            await login.userLogin(blankString, adminPassword);
        }

        [TestMethod]
        public async Task Login5_InvalidPassword_ValidUsername()
        {
            await login.userLogin(adminUsername, invalidPassword);
        }

        [TestMethod]
        public async Task Login6_BlankPassword_ValidUsername()
        {
            await login.userLogin(adminUsername, blankString);
        }
    }
}
