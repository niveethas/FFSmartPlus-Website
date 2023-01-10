using ClientAPI;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;

namespace FFSmartPlus_Website.Pages
{
    public partial class Login
    {
        [Inject]
        public FFSBackEnd? _client { get; set; }

        public string inputUsername;
        public string inputPassword;    


        protected async override Task OnInitializedAsync()
        {
           

        }

        public async Task userLogin(string username, string password)
        {
            var newLogin = new LoginModel();
            newLogin.Username = username;
            newLogin.Password = password;
            await _client.LoginAsync(newLogin);
        }

    }
}