using ClientAPI;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using FFSmartPlus_Website.Shared;
using MatBlazor;

namespace FFSmartPlus_Website.Pages
{
    public partial class Login
    {

        [Inject]
        public NavigationManager NavManager { get; set; }
        [Inject]
        public FFSBackEnd? _client { get; set; }
        [Inject]
        protected IMatToaster Toaster { get; set; }

        public string inputUsername;
        public string inputPassword;
        public LoginRespDto loginResponse;
        public string loginSuccess = "";

        public List<string>? currentUserRole;
  
        protected async override Task OnInitializedAsync()
        {
            CurrentUserRoles? i = new CurrentUserRoles();
            currentUserRole = CurrentUserRoles._role;
            StateHasChanged();

        }

        public async Task userLogin(string username, string password)
        {
            var newLogin = new LoginModel();
            
            newLogin.Username = username;
            newLogin.Password = password;
            try
            {
                loginResponse = await _client.LoginAsync(newLogin);

                var token = new JwtSecurityToken(jwtEncodedString: loginResponse.Token.ToString());
                var temp = token.Claims.ToList();
                 _client.Authorisation(loginResponse.Token);
                CurrentUserRoles currentRole = new CurrentUserRoles();
                List<string> roles = new List<string>();

                for (int i = 0; i < temp.Count; i++)
                {
                    if ((temp[i].Value == "HeadChef") || (temp[i].Value == "Chef") || (temp[i].Value == "Delivery") || (temp[i].Value == "Admin"))
                    {
                        var roleVal = temp[i].Value;
                        roles.Add(roleVal);

                    }
                }
                CurrentUserRoles._role = roles;
                CurrentUserRoles._user = username;

                loginSuccess = "True";
                NavManager.NavigateTo("/ItemList");
            }
            catch (Exception ex)
            {
                loginSuccess = "False";
            }
        }

        public void ToasterStatus()
        {
            loginSuccess = "";

        }

        public async Task SignUpPageRedirect()
        {
            NavManager.NavigateTo("/SignUp");
        }

    }
}