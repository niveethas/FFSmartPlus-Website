﻿using ClientAPI;
using MatBlazor;
using Microsoft.AspNetCore.Components;


namespace FFSmartPlus_Website.Pages
{
    public partial class ManageUsers
    {

        [Inject]
        public FFSBackEnd? _client { get; set; }

        public ICollection<String> allUsers;

        public string inputUsername;
        public string inputPassword;
        public string inputEmail;
        public string additionSuccess;
        public bool successDelete;
        [Inject]
        protected IMatToaster Toaster { get; set; }

        public List<string> currentUserRole;

        protected async override Task OnInitializedAsync()
        {
            CurrentUserRoles? i = new CurrentUserRoles();
            currentUserRole = CurrentUserRoles._role;
            StateHasChanged();
            allUsers = await _client.AllAsync();

        }

        public async Task addUser(string username, string password, string email)
        {

            //create new instance of model that has registration information
            //assign input information to model
            try
            {
                var newRM = new RegisterModel();
                newRM.Username = username;
                newRM.Password = password;
                newRM.Email = email;
                await _client.RegisterAsync(newRM);
                await checkSuccess();
                allUsers = await _client.AllAsync();
            }
            catch(Exception e)
            {
                additionSuccess = "False";
            }
            
        }

        //bring back all users to delete the ones in a drop down list style, or search by username

        public async Task deleteUser(string username)
        {
            try
            {
                //delete user via username; it is unique
                successDelete = await _client.DeleteUserAsync(username);
                allUsers = await _client.AllAsync();
            }
            catch(Exception e)
            {
                successDelete = false;
            }
            
        }

        private async Task checkSuccess()
        {
            var currentCount = allUsers.Count;
            allUsers = await _client.AllAsync();
            if (currentCount < allUsers.Count)
            {
                 additionSuccess = "True";
            }
            else
            {
                additionSuccess = "False";
            }
        }

        public void ToasterStatus()
        {
            additionSuccess = "";

        }

    }
}
