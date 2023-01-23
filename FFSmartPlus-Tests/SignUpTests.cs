﻿using ClientAPI;
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
    public class SignUpTests
    {
        private SignUp signUp;

        private String validUsername = "SignUpUser";
        private String validPassword = "@SU_tests123";
        private String validEmail = "Email@email.com";

        private String validUsernameToo = "SUUser2";

        private String blankString = "";

        private bool testsComplete = false;

        public SignUpTests()
        {
            signUp = new SignUp();
            signUp._client = new FFSBackEnd("https://localhost:7041/", new HttpClient());
        }

        // --- userSignUp(string username, string password, string email) ---
        [TestMethod]
        public async Task SignUp_ValidUser()
        {
            await signUp._client.DeleteUserAsync(validUsername);
            await signUp.userSignUp(validUsername, validPassword, validEmail);
        }

        [TestMethod]
        public async Task SignUp_InvalidUser_ExistingUser()
        {
            await signUp.userSignUp(validUsername, validPassword, validEmail);
        }

        [TestMethod]
        public async Task SignUp_InvalidPassword_NoNumbers()
        {
            String invalidPassword = "@Password";
            await signUp.userSignUp(validUsernameToo, invalidPassword, validEmail);
        }

        [TestMethod]
        public async Task SignUp_InvalidPassword_NoSpecial()
        {
            String invalidPassword = "Password123";
            await signUp.userSignUp(validUsernameToo, invalidPassword, validEmail);
        }

        [TestMethod]
        public async Task SignUp_InvalidPassword_NoCapital()
        {
            String invalidPassword = "@password123";
            await signUp.userSignUp(validUsernameToo, invalidPassword, validEmail);
        }

        [TestMethod]
        public async Task SignUp_InvalidPassword_Blank()
        {
            await signUp.userSignUp(validUsernameToo, blankString, validEmail);
        }

        [TestMethod]
        public async Task SignUp_InvalidUsername_Blank()
        {
            await signUp.userSignUp(blankString, validPassword, validEmail);
        }

        [TestMethod]
        public async Task SignUp_InvalidEmail_InvalidFormat()
        {
            String invalidEmail = "test";
            await signUp.userSignUp(validUsernameToo, validPassword, invalidEmail);
        }

        [TestMethod]
        public async Task SignUp_InvalidEmail_Blank()
        {
            await signUp.userSignUp(validUsernameToo, validPassword, blankString);
            testsComplete = true;
        }

    }
}
