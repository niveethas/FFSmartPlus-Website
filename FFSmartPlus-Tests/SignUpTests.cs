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
        private SignUp signUp;

        private string validUsername = "SignUpUser";
        private string validPassword = "@SU_tests123";
        private string validEmail = "Email@email.com";

        private string validUsernameToo = "SUUser2";

        private string blankString = "";

        public SignUpTests()
        {
            signUp = new SignUp();
            signUp._client = new FFSBackEnd("https://localhost:7041/", new HttpClient());
            signUp.NavManager = new TestNav();
        }

        // --- userSignUp(string username, string password, string email) ---
        [TestMethod]
        public async Task TA1_SignUp_ValidUser()
        {
            await signUp._client.DeleteUserAsync(validUsername);
            await signUp.userSignUp(validUsername, validPassword, validEmail, validPassword);

            Assert.AreEqual(TestConsts.TRUE_STR, signUp.signUpSuccess);
        }

        [TestMethod]
        public async Task TA2_SignUp_InvalidUser_ExistingUser()
        {
            await signUp.userSignUp(validUsername, validPassword, validEmail, validPassword);
            Assert.AreEqual(TestConsts.FALSE_STR, signUp.signUpSuccess);
        }

        [TestMethod]
        public async Task TA3_SignUp_InvalidPassword_NoNumbers()
        {
            string invalidPassword = "@Password";
            await signUp.userSignUp(validUsernameToo, invalidPassword, validEmail, invalidPassword);
            Assert.AreEqual(TestConsts.FALSE_STR, signUp.signUpSuccess);
        }

        [TestMethod]
        public async Task TA4_SignUp_InvalidPassword_NoSpecial()
        {
            string invalidPassword = "Password123";
            await signUp.userSignUp(validUsernameToo, invalidPassword, validEmail, invalidPassword);
            Assert.AreEqual(TestConsts.FALSE_STR, signUp.signUpSuccess);
        }

        [TestMethod]
        public async Task TA5_SignUp_InvalidPassword_NoCapital()
        {
            string invalidPassword = "@password123";
            await signUp.userSignUp(validUsernameToo, invalidPassword, validEmail, invalidPassword);
            Assert.AreEqual(TestConsts.FALSE_STR, signUp.signUpSuccess);
        }

        [TestMethod]
        public async Task TA6_SignUp_InvalidPassword_Blank()
        {
            await signUp.userSignUp(validUsernameToo, blankString, validEmail, blankString);
            Assert.AreEqual(TestConsts.FALSE_STR, signUp.signUpSuccess);
        }

        [TestMethod]
        public async Task TA7_SignUp_InvalidUsername_Blank()
        {
            await signUp.userSignUp(blankString, validPassword, validEmail, validPassword);
            Assert.AreEqual(TestConsts.FALSE_STR, signUp.signUpSuccess);
        }

        [TestMethod]
        public async Task TA8_SignUp_InvalidEmail_InvalidFormat()
        {
            string invalidEmail = "test";
            await signUp.userSignUp(validUsernameToo, validPassword, invalidEmail, validPassword);
            Assert.AreEqual(TestConsts.FALSE_STR, signUp.signUpSuccess);
        }

        [TestMethod]
        public async Task TA9_SignUp_InvalidEmail_Blank()
        {
            await signUp.userSignUp(validUsernameToo, validPassword, blankString, validPassword);
            Assert.AreEqual(TestConsts.FALSE_STR, signUp.signUpSuccess);
        }


        [TestMethod]
        public async Task TA9_SignUp_InvalidPassword_NotMatching()
        {
            string validPasswordAlt = "@PasswordToo123";
            await signUp.userSignUp(validUsernameToo, validPassword, blankString, validPasswordAlt);
            Assert.AreEqual(TestConsts.FALSE_STR, signUp.signUpSuccess);
        }

        [TestMethod]
        public async Task TestCleanup_DeleteNewUser()
        {
            await signUp._client.DeleteUserAsync(validUsername);
        }
    }
}
