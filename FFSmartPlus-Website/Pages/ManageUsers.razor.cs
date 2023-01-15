using ClientAPI;
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
        public bool additionSuccess;
        public bool successDelete;
        [Inject]
        protected IMatToaster Toaster { get; set; }

        protected async override Task OnInitializedAsync()
        {
            allUsers = await _client.AllAsync();
        }

        public async Task addUser(string username, string password, string email)
        {
            //create new instance of model that has registration information
            //assign input information to model
            var newRM = new RegisterModel();
            newRM.Username = username;
            newRM.Password = password;
            newRM.Email = email;
            await _client.RegisterAsync(newRM);
            await checkSuccess();
            allUsers = await _client.AllAsync();
            
        }

        //bring back all users to delete the ones in a drop down list style, or search by username

        public async Task deleteUser(string username)
        {
            //delete user via username; it is unique
            successDelete =  await _client.DeleteUserAsync(username);
            allUsers = await _client.AllAsync();
            
        }

        private async Task checkSuccess()
        {
            var currentCount = allUsers.Count;
            allUsers = await _client.AllAsync();
           if (currentCount < allUsers.Count)
            {
                 additionSuccess = true;
            }
            else
            {
                additionSuccess = false;
            }
        }

        

    }
}
