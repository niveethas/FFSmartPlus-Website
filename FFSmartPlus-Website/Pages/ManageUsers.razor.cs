using ClientAPI;
using Microsoft.AspNetCore.Components;

namespace FFSmartPlus_Website.Pages
{
    public partial class ManageUsers
    {

        [Inject]
        public FFSBackEnd? _client { get; set; }

        public string inputUsername;
        public string inputPassword;
        public string inputEmail;

        public async Task addUser(string username, string password, string email)
        {
            var newRM = new RegisterModel();
            newRM.Username = username;
            newRM.Password = password;
            newRM.Email = email;
            await _client.RegisterAsync(newRM);
        }

        public async Task deleteUser()
        {
            //nick will make back end code
            //then complete this
            //make pop up when the code is complete (provide success or failure message)
        }

    }
}
