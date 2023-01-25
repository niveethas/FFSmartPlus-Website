using ClientAPI;
using MatBlazor;
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

        [Inject]
        protected IMatToaster Toaster { get; set; }

        public string inputUsername;
        public string inputPassword;
        public string inputEmail;
        public string signUpSuccess;

        public async Task userSignUp(string username, string password, string email)
        {
            try
            {
                var newRM = new RegisterModel();
                newRM.Username = username;
                newRM.Password = password;
                newRM.Email = email;
                await _client.RegisterAsync(newRM);

                ICollection<string>users = await _client.AllAsync();
                if (users.Contains(username))
                {
                    signUpSuccess = "True";
                    NavManager.NavigateTo("/ItemList");
                }
                else
                {
                    signUpSuccess = "False";
                }
            }
            catch(Exception e)
            {
                signUpSuccess = "False";
            }

        }


        public async Task ToasterStatus()
        {
            signUpSuccess = "";

        }


    }
}
