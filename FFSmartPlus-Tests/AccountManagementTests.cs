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

            // populating current user to avoid errors
            CurrentUserRoles currentUser = new CurrentUserRoles() { Role = new List<string> { "admin", "chef" }, User = "admin1" };
            _accountManagement.currentUser = currentUser;

        }


        [TestMethod]
        public async Task T0_GetUsers_Valid()
        {
            // Gets all valid users 
            await _accountManagement.getUsers();

            Assert.IsTrue(_accountManagement.users.Count > 0);

            // Get first user from the user list for validUsername to be used in later tests
            validUsername = _accountManagement.users.First();
        }

        #region Admin

        [TestMethod]
        public async Task TA1_addRole_Valid_Admin()
        {
            // Adds admin role to user
            string role = roles[0];

            // deletes role from user to ensure they dont have it already
            await _accountManagement.deleteRole(validUsername, role);

            await _accountManagement.addRole(validUsername, role);

            Assert.AreEqual(TestConsts.TRUE_STR, _accountManagement.success);
        }

        [TestMethod]
        public async Task TA2_addRole_Invalid_AlreadyHave_Admin()
        {
            // adds admin role to user who already has admin role (from previous tc)
            string role = roles[0];
            await _accountManagement.addRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, _accountManagement.success);
        }


        [TestMethod]
        public async Task TA3_deleteRole_Valid_Admin()
        {
            // deletes admin role from user from previous tc
            string role = roles[0];
            await _accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(TestConsts.TRUE_STR, _accountManagement.success);
        }

        [TestMethod]
        public async Task TA4_deleteRole_Invalid_DontHave_Admin()
        {
            // deletes admin role from user who does not have it, from previous tc 
            string role = roles[0];
            await _accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, _accountManagement.success);
        }

        #endregion

        #region Chef
        // Repeating the previous add-add-remove-remove pattern for the chef role
        [TestMethod]
        public async Task TB1_addRole_Valid_Chef()
        {
            // add chef role to user who does not have the chef role

            string role = roles[1];

            // deletes role from user to ensure they don't have it already
            await _accountManagement.deleteRole(validUsername, role);

            await _accountManagement.addRole(validUsername, role);

            Assert.AreEqual(TestConsts.TRUE_STR, _accountManagement.success);
        }

        [TestMethod]
        public async Task TB2_addRole_Invalid_AlreadyHave_Chef()
        {
            // add chef role to user who already has it (from previous tc)
            string role = roles[1];
            await _accountManagement.addRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, _accountManagement.success);
        }

        [TestMethod]
        public async Task TB3_deleteRole_Valid_Chef()
        {
            // deletes chef role from user - who has it from previous tcs
            string role = roles[1];
            await _accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(TestConsts.TRUE_STR, _accountManagement.success);
        }

        [TestMethod]
        public async Task TB4_deleteRole_Invalid_DontHave_Chef()
        {
            // deletes chef role from user who does not have it - due to previous tc
            string role = roles[1];
            await _accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, _accountManagement.success);
        }

        #endregion


        #region Delivery
        // same add-add-remove-remove pattern but for the delivery role
        [TestMethod]
        public async Task TC1_addRole_Valid_Delivery()
        {
            // add delivery role to user
            string role = roles[2];

            // delete role first to ensure this is the first instance
            await _accountManagement.deleteRole(validUsername, role);

            await _accountManagement.addRole(validUsername, role);

            Assert.AreEqual(TestConsts.TRUE_STR, _accountManagement.success);
        }

        [TestMethod]
        public async Task TC2_addRole_Invalid_AlreadyHave_Delivery()
        {
            // adding delivery role to user who already has it from previous tc
            string role = roles[2];
            await _accountManagement.addRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, _accountManagement.success);
        }

        [TestMethod]
        public async Task TC3_deleteRole_Valid_Delivery()
        {
            // deletes delivery role from user - added in previous tc
            string role = roles[2];
            await _accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(TestConsts.TRUE_STR, _accountManagement.success);
        }

        [TestMethod]
        public async Task TC4_deleteRole_Invalid_DontHave_Delivery()
        {
            // deletes role from user who does not have it due to previous tc
            string role = roles[2];
            await _accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, _accountManagement.success);
        }

        #endregion

        #region error tests
        // testin invalid inputs
        [TestMethod]
        public async Task TD1_addRole_Invalid_Role()
        {
            // trying to add user to role that does not exist
            string role = "adghf";
            await _accountManagement.addRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, _accountManagement.success);
        }

        [TestMethod]
        public async Task TD2_addRole_Invalid_User()
        {
            // trying to add role to user that does not exist
            string role = roles[2];
            string username = "askmlfkl";
            await _accountManagement.addRole(username, role);

            Assert.AreEqual(TestConsts.FALSE_STR, _accountManagement.success);
        }

        [TestMethod]
        public async Task TD3_deleteRole_Invalid_Role()
        {
            // deleting a role from a user where the role does not exist
            string role = "adghf";
            await _accountManagement.deleteRole(validUsername, role);

            Assert.AreEqual(TestConsts.FALSE_STR, _accountManagement.success);
        }

        [TestMethod]
        public async Task TD4_deleteRole_Invalid_User()
        {
            // deleting role from user that does not exist
            string role = roles[2];
            string username = "askjakgkl";
            await _accountManagement.deleteRole(username, role);

            Assert.AreEqual(TestConsts.FALSE_STR, _accountManagement.success);
        }
        #endregion


    }
}
