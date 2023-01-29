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
            // add new user - valid details
            // deletes user first to ensure they are being added
            await _manageUsers.deleteUser(validUsername);

            await _manageUsers.addUser(validUsername, validPassword, validEmail);
            Assert.AreEqual(TestConsts.TRUE_STR, _manageUsers.additionSuccess);
            CollectionAssert.Contains((System.Collections.ICollection)_manageUsers.allUsers, validUsername);
        }

        [TestMethod]
        public async Task TA2_AddUser_ExistingUser()
        {
            // add user that already exists - from previous tc
            await _manageUsers.addUser(validUsername, validPassword, validEmail);
            Assert.AreEqual(TestConsts.FALSE_STR, _manageUsers.additionSuccess);
        }

        [TestMethod]
        public async Task TA3_AddUser_InvalidPassword_NoNumbers()
        {
            // 
            string invalidPassword = "@Password";
            await _manageUsers.addUser(validUsernameToo, invalidPassword, validEmail);
            Assert.AreEqual(TestConsts.FALSE_STR, _manageUsers.additionSuccess);

            _manageUsers.additionSuccess = null;
        }

        [TestMethod]
        public async Task TA4_AddUser_InvalidPassword_NoSpecial()
        {
            // invalid password - no special character
            string invalidPassword = "Password123";
            await _manageUsers.addUser(validUsernameToo, invalidPassword, validEmail);
            Assert.AreEqual(TestConsts.FALSE_STR, _manageUsers.additionSuccess);

            _manageUsers.additionSuccess = null;
        }

        [TestMethod]
        public async Task TA5_AddUser_InvalidPassword_NoCapital()
        {
            // invalid password - no capital letter
            string invalidPassword = "@password123";
            await _manageUsers.addUser(validUsernameToo, invalidPassword, validEmail);
            Assert.AreEqual(TestConsts.FALSE_STR, _manageUsers.additionSuccess);

            _manageUsers.additionSuccess = null;
        }

        [TestMethod]
        public async Task TA6_AddUser_InvalidPassword_Blank()
        {
            // invalid password - no value
            await _manageUsers.addUser(validUsernameToo, blankString, validEmail);
            Assert.AreEqual(TestConsts.FALSE_STR, _manageUsers.additionSuccess);

            _manageUsers.additionSuccess = null;
        }

        [TestMethod]
        public async Task TA7_AddUser_InvalidUsername_Blank()
        {
            // invalid username - no value
            await _manageUsers.addUser(blankString, validPassword, validEmail);
            Assert.AreEqual(TestConsts.FALSE_STR, _manageUsers.additionSuccess);

            _manageUsers.additionSuccess = null;
        }

        [TestMethod]
        public async Task TA8_AddUser_InvalidEmail_InvalidFormat()
        {
            // invalid email - incorrect email format - not [name]@[address].[etc]
            string invalidEmail = "test";
            await _manageUsers.addUser(validUsernameToo, validPassword, invalidEmail);
            Assert.AreEqual(TestConsts.FALSE_STR, _manageUsers.additionSuccess);

            _manageUsers.additionSuccess = null;
        }

        [TestMethod]
        public async Task TA9_AddUser_InvalidEmail_Blank()
        {
            // invalid email - no email
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
            // delete user - valid user - added in first tc
            await _manageUsers.deleteUser(validUsername);
            Assert.IsTrue(_manageUsers.successDelete);
            CollectionAssert.DoesNotContain((System.Collections.ICollection)_manageUsers.allUsers, validUsername);
        }

        [TestMethod]
        public async Task TB2_DeleteUser_InvalidUser_NonExistent()
        {
            // delete user - user that does not exist - deleted in previous tc
            await _manageUsers.deleteUser(validUsername);
            Assert.IsFalse(_manageUsers.successDelete);
        }

        [TestMethod]
        public async Task TB3_DeleteUser_InvalidUser_Blank()
        {
            // attempt to delete user - blank string
            await _manageUsers.deleteUser(blankString);
            Assert.IsFalse(_manageUsers.successDelete);
        }
        #endregion
    }
}
