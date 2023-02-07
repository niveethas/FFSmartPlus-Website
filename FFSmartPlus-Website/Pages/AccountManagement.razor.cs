using ClientAPI;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Linq;

namespace FFSmartPlus_Website.Pages
{
    public partial class AccountManagement
    {
        [Inject]
        public FFSBackEnd? _client { get; set; }
        [Inject]
        protected IMatToaster Toaster { get; set; }

        public CurrentUserRoles currentUser = new CurrentUserRoles();
        public string inputUsername;
        public string inputRole;
        public string selectedUser;
        public string? success = "";
        public ICollection<string>? users = new List<string>();


        public List<string> currentUserRole;

        protected async override Task OnInitializedAsync()
        {
            CurrentUserRoles? i = new CurrentUserRoles();
            currentUserRole = CurrentUserRoles._role;
            StateHasChanged();

            await getUsers();
            //load up all the users currently in the database to display in the dropdown menu

        }

        public async Task getUsers()
        {
                users = await _client.AllAsync();
            //calls the get method to retrieve a list collection of users
        }

        protected void OnChangeUser(string user)
        {
            inputUsername = user;
            //changes the name in textbox based on the one clicked in the dropdown menu
        }

        protected List<string> getUsersToList()
        {
            return users.ToList();
            //modifies collection to list, for use in razor page with javascript components. 
        }

        public async Task addRole(string username, string role)
        {
            try
            {
                //call the add role method and pass in user choices as string parameters
                await _client.AddRoleAsync(username, role);
                var checkUser = CurrentUserRoles._user.ToLower();
                if (username.ToLower() == checkUser)
                {
                    var currRoles = new List<string>();
                    currRoles = CurrentUserRoles._role;
                    try
                    {
                        currRoles.Add(role);
                        //change the current roles based on the input, if it is for the user that is currently logged in. 
                        CurrentUserRoles._role = currRoles;
                        success = "TrueSelf";
                    }
                    catch
                    {
                        success = "False";

                    }
                    //sets the appropriate toast to show based on success of role modification
                }
                else
                {
                    success = "True";

                }
            }
            catch (Exception ex){
                success = "False";
            }
        }



        public async Task deleteRole(string username, string role)
        {
            try
            {
                await _client.RemoveRoleAsync(username, role);
                var checkUser = CurrentUserRoles._user.ToLower();
                if (username.ToLower() == checkUser)
                {
                    var currRoles = new List<string>();
                    currRoles = CurrentUserRoles._role;
                    try
                    {
                        currRoles.Remove(role);
                        CurrentUserRoles._role = currRoles;
                        //change the current roles based on the input, if it is for the user that is currently logged in. 
                        success = "TrueSelf";
                        //sets the appropriate toast to show based on success of role modification
                    }
                    catch
                    {
                        success = "False";
                    }
                }
                else
                {
                    success = "True";
                }
            }
            catch (Exception ex)
            {
                success = "False";

            }
        }

        public void ToasterStatus()
        {
            success = "";

        }
    }
}
