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
        private readonly ManageUsers _manageUsers;

        private string validUsername = "UnitUser";
        private string validPassword = "@UnitTests123";
        private string validEmail = "Email@email.com";

        private string validUsernameToo = "UnitUser2";

        private string blankString = "";

        public ManageUsersTests()
        {
            _manageUsers = new ManageUsers();
            _manageUsers._client = new SetupClient().SignInAdmin();

            Task<ICollection<String>> allUsers = _manageUsers._client.AllAsync();
            _manageUsers.allUsers = allUsers.Result;
        }


        #region AddUser Tests
        // --- addUser(string username, string password, string email) Tests ---

        [TestMethod]
        public async Task TA1_AddUser_ValidUser()
        {
            await _manageUsers.addUser(validUsername, validPassword, validEmail);
            Assert.AreEqual(TestConsts.TRUE_STR, _manageUsers.additionSuccess);
            CollectionAssert.Contains((System.Collections.ICollection)_manageUsers.allUsers, validUsername);

            //manageUsers.additionSuccess = null;

        }

        [TestMethod]
        public async Task TA2_AddUser_ExistingUser()
        {
            await _manageUsers.addUser(validUsername, validPassword, validEmail);
            Assert.AreEqual(TestConsts.FALSE_STR, _manageUsers.additionSuccess);
            //manageUsers.additionSuccess = null;
        }

        [TestMethod]
        public async Task TA3_AddUser_InvalidPassword_NoNumbers()
        {
            String invalidPassword = "@Password";
            await _manageUsers.addUser(validUsernameToo, invalidPassword, validEmail);
            Assert.AreEqual(TestConsts.FALSE_STR, _manageUsers.additionSuccess);

            _manageUsers.additionSuccess = null;
        }

        [TestMethod]
        public async Task TA4_AddUser_InvalidPassword_NoSpecial()
        {
            String invalidPassword = "Password123";
            await _manageUsers.addUser(validUsernameToo, invalidPassword, validEmail);
            Assert.AreEqual(TestConsts.FALSE_STR, _manageUsers.additionSuccess);

            _manageUsers.additionSuccess = null;
        }

        [TestMethod]
        public async Task TA5_AddUser_InvalidPassword_NoCapital()
        {
            String invalidPassword = "@password123";
            await _manageUsers.addUser(validUsernameToo, invalidPassword, validEmail);
            Assert.AreEqual(TestConsts.FALSE_STR, _manageUsers.additionSuccess);

            _manageUsers.additionSuccess = null;
        }

        [TestMethod]
        public async Task TA6_AddUser_InvalidPassword_Blank()
        {
            await _manageUsers.addUser(validUsernameToo, blankString, validEmail);
            Assert.AreEqual(TestConsts.FALSE_STR, _manageUsers.additionSuccess);

            _manageUsers.additionSuccess = null;
        }

        [TestMethod]
        public async Task TA7_AddUser_InvalidUsername_Blank()
        {
            await _manageUsers.addUser(blankString, validPassword, validEmail);
            Assert.AreEqual(TestConsts.FALSE_STR, _manageUsers.additionSuccess);

            _manageUsers.additionSuccess = null;
        }

        [TestMethod]
        public async Task TA8_AddUser_InvalidEmail_InvalidFormat()
        {
            String invalidEmail = "test";
            await _manageUsers.addUser(validUsernameToo, validPassword, invalidEmail);
            Assert.AreEqual(TestConsts.FALSE_STR, _manageUsers.additionSuccess);

            _manageUsers.additionSuccess = null;
        }

        [TestMethod]
        public async Task TA9_AddUser_InvalidEmail_Blank()
        {
            await _manageUsers.addUser(validUsernameToo, validPassword, blankString);
            Assert.AreEqual(TestConsts.FALSE_STR, _manageUsers.additionSuccess);

            _manageUsers.additionSuccess = null;
        }
        #endregion

        #region DeleteUser Tests
        // --- deleteUser(string username) Tests --- 


        [TestMethod]
        public async Task TB1_DeleteUser_ValidUser()
        {
            await _manageUsers.deleteUser(validUsername);
            Assert.IsTrue(_manageUsers.successDelete);
            CollectionAssert.DoesNotContain((System.Collections.ICollection)_manageUsers.allUsers, validUsername);
        }

        [TestMethod]
        public async Task TB2_DeleteUser_InvalidUser_NonExistent()
        {
            await _manageUsers.deleteUser(validUsername);
            Assert.IsFalse(_manageUsers.successDelete);
        }

        [TestMethod]
        public async Task TB3_DeleteUser_InvalidUser_Blank()
        {
            await _manageUsers.deleteUser(blankString);
            Assert.IsFalse(_manageUsers.successDelete);
        }
        #endregion
    }
}
