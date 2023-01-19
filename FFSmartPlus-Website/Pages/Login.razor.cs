using ClientAPI;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using FFSmartPlus_Website.Shared;

namespace FFSmartPlus_Website.Pages
{
    public partial class Login
    {

        [Inject]
        public NavigationManager NavManager { get; set; }


        [Inject]
        public FFSBackEnd? _client { get; set; }

        public string inputUsername;
        public string inputPassword;
        public LoginRespDto loginResponse;


        protected async override Task OnInitializedAsync()
        {
           

        }

        public async Task userLogin(string username, string password)
        {
            var newLogin = new LoginModel();
            
            newLogin.Username = username;
            newLogin.Password = password;
            loginResponse =  await _client.LoginAsync(newLogin);

            var token = new JwtSecurityToken(jwtEncodedString: loginResponse.Token.ToString());
            var temp = token.Claims.ToList();
            var tempcount = 0;
            CurrentUserRoles currentRole = new CurrentUserRoles();
            List<string> roles = new List<string>();

            for (int i = 0; i < temp.Count; i++){
                if ((temp[i].Value == "HeadChef") || (temp[i].Value == "Chef") || (temp[i].Value == "Delivery") || (temp[i].Value == "Admin"))
                {
                    var temp3 = temp[i].Value;
                    roles.Add(temp3);
                                        
                }
            }
            CurrentUserRoles._role = roles;
            NavMenu Nm = new NavMenu();
            Nm.GetRoles();
            NavManager.NavigateTo("/ItemList");
        }


        public async Task SignUpPageRedirect()
        {
            NavManager.NavigateTo("/SignUp");
        }

    }
}