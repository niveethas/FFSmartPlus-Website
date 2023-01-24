using ClientAPI;
using FFSmartPlus_Website.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFSmartPlus_Tests
{
    [TestClass]
    public class ManageUsersTests
    {
        private readonly ManageUsers manageUsers;

        private string validUsername = "UnitUser";
        private string validPassword = "@UnitTests123";
        private string validEmail = "Email@email.com";

        private string validUsernameToo = "UnitUser2";

        private string blankString = "";

        private string trueStr = "true";
        private string falseStr = "false";

        public ManageUsersTests()
        {
            manageUsers = new ManageUsers();
            manageUsers._client = new FFSBackEnd("https://localhost:7041/", new HttpClient());

            var newLogin = new LoginModel();
            newLogin.Username = "admin1";
            newLogin.Password = "@Admin123"; // Password requires a symbol, capital letter and 3 numbers
            manageUsers._client.LoginAsync(newLogin);
            var loginCode = manageUsers._client.LoginAsync(newLogin);
            manageUsers._client.AddAuth(loginCode.Result.Token);

            Task<ICollection<String>> allUsers = manageUsers._client.AllAsync();
            manageUsers.allUsers = allUsers.Result;
        }


        #region AddUser Tests
        // --- addUser(string username, string password, string email) Tests ---

        [TestMethod]
        public async Task AddUser1_ValidUser()
        {
            await manageUsers.addUser(validUsername, validPassword, validEmail);
            Assert.AreEqual(trueStr, manageUsers.additionSuccess);
            CollectionAssert.Contains((System.Collections.ICollection)manageUsers.allUsers, validUsername);

            //manageUsers.additionSuccess = null;

        }

        [TestMethod]
        public async Task AddUser2_ExistingUser()
        {
            await manageUsers.addUser(validUsername, validPassword, validEmail);
            Assert.AreEqual(falseStr, manageUsers.additionSuccess);
            //manageUsers.additionSuccess = null;
        }

        [TestMethod]
        public async Task AddUser3_InvalidPassword_NoNumbers()
        {
            String invalidPassword = "@Password";
            await manageUsers.addUser(validUsernameToo, invalidPassword, validEmail);
            Assert.AreEqual(falseStr, manageUsers.additionSuccess);

            manageUsers.additionSuccess = null;
        }

        [TestMethod]
        public async Task AddUser4_InvalidPassword_NoSpecial()
        {
            String invalidPassword = "Password123";
            await manageUsers.addUser(validUsernameToo, invalidPassword, validEmail);
            Assert.AreEqual(falseStr, manageUsers.additionSuccess);

            manageUsers.additionSuccess = null;
        }

        [TestMethod]
        public async Task AddUser5_InvalidPassword_NoCapital()
        {
            String invalidPassword = "@password123";
            await manageUsers.addUser(validUsernameToo, invalidPassword, validEmail);
            Assert.AreEqual(falseStr, manageUsers.additionSuccess);

            manageUsers.additionSuccess = null;
        }

        [TestMethod]
        public async Task AddUser6_InvalidPassword_Blank()
        {
            await manageUsers.addUser(validUsernameToo, blankString, validEmail);
            Assert.AreEqual(falseStr, manageUsers.additionSuccess);

            manageUsers.additionSuccess = null;
        }

        [TestMethod]
        public async Task AddUser7_InvalidUsername_Blank()
        {
            await manageUsers.addUser(blankString, validPassword, validEmail);
            Assert.AreEqual(falseStr, manageUsers.additionSuccess);

            manageUsers.additionSuccess = null;
        }

        [TestMethod]
        public async Task AddUser8_InvalidEmail_InvalidFormat()
        {
            String invalidEmail = "test";
            await manageUsers.addUser(validUsernameToo, validPassword, invalidEmail);
            Assert.AreEqual(falseStr, manageUsers.additionSuccess);

            manageUsers.additionSuccess = null;
        }

        [TestMethod]
        public async Task AddUser9_InvalidEmail_Blank()
        {
            await manageUsers.addUser(validUsernameToo, validPassword, blankString);
            Assert.AreEqual(falseStr, manageUsers.additionSuccess);

            manageUsers.additionSuccess = null;
        }
        #endregion

        #region DeleteUser Tests
        // --- deleteUser(string username) Tests --- 


        [TestMethod]
        public async Task DeleteUser1_ValidUser()
        {
            await manageUsers.deleteUser(validUsername);
            Assert.IsTrue(manageUsers.successDelete);
            CollectionAssert.DoesNotContain((System.Collections.ICollection)manageUsers.allUsers, validUsername);
        }

        [TestMethod]
        public async Task DeleteUser2_InvalidUser_NonExistent()
        {
            await manageUsers.deleteUser(validUsername);
            Assert.IsFalse(manageUsers.successDelete);
        }

        [TestMethod]
        public async Task DeleteUser3_InvalidUser_Blank()
        {
            await manageUsers.deleteUser(blankString);
            Assert.IsFalse(manageUsers.successDelete);
        }
        #endregion
    }
}
