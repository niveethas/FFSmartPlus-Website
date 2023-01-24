﻿using ClientAPI;
using Microsoft.AspNetCore.Components;
using System.IdentityModel.Tokens.Jwt;


namespace FFSmartPlus_Website.Pages
{
    public partial class SignUp
    {

        [Inject]
        public NavigationManager NavManager { get; set; }
        [Inject]
        public FFSBackEnd? _client { get; set; }

        public string inputUsername;
        public string inputPassword;
        public string inputEmail;

        public async Task userSignUp(string username, string password, string email)
        {
            var newRM = new RegisterModel();
            newRM.Username = username;
            newRM.Password = password;
            newRM.Email = email;
            await _client.RegisterAsync(newRM);
            NavManager.NavigateTo("/ItemList");

        }
    }
}
