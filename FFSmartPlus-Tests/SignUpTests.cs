using ClientAPI;
using FFSmartPlus_Website.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFSmartPlus_Tests
{
    [TestClass]
    public class SignUpTests
    {
        private SignUp _signUp;

        private string validUsername = "SignUpUser";
        private string validPassword = "@SU_tests123";
        private string validEmail = "Email@email.com";

        private string validUsernameToo = "SUUser2";

        private string blankString = "";

        public SignUpTests()
        {
            _signUp = new SignUp();
            _signUp._client = new FFSBackEnd("https://localhost:7041/", new HttpClient());
            _signUp.NavManager = new TestNav();
        }


        // --- userSignUp(string username, string password, string email) ---
        [TestMethod]
        public async Task TA1_SignUp_ValidUser()
        {
            // sign up with valid details
            await _signUp.userSignUp(validUsername, validPassword, validEmail, validPassword);

            Assert.AreEqual(TestConsts.TRUE_STR, _signUp.signUpSuccess);
        }

        [TestMethod]
        public async Task TA2_SignUp_InvalidUser_ExistingUser()
        {
            // sign up with existing user - created in last tc
            await _signUp.userSignUp(validUsername, validPassword, validEmail, validPassword);
            Assert.AreEqual(TestConsts.FALSE_STR, _signUp.signUpSuccess);
        }

        [TestMethod]
        public async Task TA3_SignUp_InvalidPassword_NoNumbers()
        {
            // sign up - invalid password - no numerical character
            string invalidPassword = "@Password";
            await _signUp.userSignUp(validUsernameToo, invalidPassword, validEmail, invalidPassword);
            Assert.AreEqual(TestConsts.FALSE_STR, _signUp.signUpSuccess);
        }

        [TestMethod]
        public async Task TA4_SignUp_InvalidPassword_NoSpecial()
        {
            // sign up - invalid password - no special character
            string invalidPassword = "Password123";
            await _signUp.userSignUp(validUsernameToo, invalidPassword, validEmail, invalidPassword);
            Assert.AreEqual(TestConsts.FALSE_STR, _signUp.signUpSuccess);
        }

        [TestMethod]
        public async Task TA5_SignUp_InvalidPassword_NoCapital()
        {
            // sign up - invalid password - no capital letter
            string invalidPassword = "@password123";
            await _signUp.userSignUp(validUsernameToo, invalidPassword, validEmail, invalidPassword);
            Assert.AreEqual(TestConsts.FALSE_STR, _signUp.signUpSuccess);
        }

        [TestMethod]
        public async Task TA6_SignUp_InvalidPassword_Blank()
        {
            // sign up - invalid password - no password
            await _signUp.userSignUp(validUsernameToo, blankString, validEmail, blankString);
            Assert.AreEqual(TestConsts.FALSE_STR, _signUp.signUpSuccess);
        }

        [TestMethod]
        public async Task TA7_SignUp_InvalidUsername_Blank()
        {
            // sign up - invalid username - no username
            await _signUp.userSignUp(blankString, validPassword, validEmail, validPassword);
            Assert.AreEqual(TestConsts.FALSE_STR, _signUp.signUpSuccess);
        }

        [TestMethod]
        public async Task TA8_SignUp_InvalidEmail_InvalidFormat()
        {
            // sign up - invalid email - invalid format - not [name]@[address].[etc]
            string invalidEmail = "test";
            await _signUp.userSignUp(validUsernameToo, validPassword, invalidEmail, validPassword);
            Assert.AreEqual(TestConsts.FALSE_STR, _signUp.signUpSuccess);
        }

        [TestMethod]
        public async Task TA9_SignUp_InvalidEmail_Blank()
        {
            // sign up - invalid email - no email
            await _signUp.userSignUp(validUsernameToo, validPassword, blankString, validPassword);
            Assert.AreEqual(TestConsts.FALSE_STR, _signUp.signUpSuccess);
        }


        [TestMethod]
        public async Task TA9_SignUp_InvalidRepeatPassword_NotMatching()
        {
            // sign up - invalid repeat password - not matching password
            string validPasswordAlt = "@PasswordToo123";
            await _signUp.userSignUp(validUsernameToo, validPassword, blankString, validPasswordAlt);
            Assert.AreEqual("FalsePassword", _signUp.signUpSuccess);
        }

        [TestMethod]
        public async Task TestCleanup_DeleteNewUser()
        {
            // delete added user to clean up from test
            _signUp._client = new SetupClient().SignInAdmin();
            await _signUp._client.DeleteUserAsync(validUsername);
        }
    }
}
