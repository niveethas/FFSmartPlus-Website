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
        public string inputPasswordR;
        public string signUpSuccess;

        public async Task userSignUp(string username, string password, string email, string passwordR)
        {
            
                var newRM = new RegisterModel();
                newRM.Username = username;
                newRM.Password = password;
                newRM.Email = email;

            if (passwordR == password)
            {
                try
                {
                    await _client.RegisterAsync(newRM);
                    signUpSuccess = "True";
                }
                catch
                {
                    signUpSuccess = "False";

                }
            }
            else
            {
                signUpSuccess = "FalsePassword";

            }
        }
            
        public async Task ToasterStatus()
        {
            signUpSuccess = "";

        }


    }
}
