using FFSmartPlus_Website.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Internal;

namespace FFSmartPlus_Website.Shared
{
    public partial class NavMenu
{
        public List<string> currentUserRole;
        public string temp;

        [Inject]
        public NavigationManager uriHelper { get; set; }

        //protected override async Task OnInitializedAsync()
        //{
            
            
        //}

        protected async void OnAfterRender(bool firstRender)
        {
            Console.WriteLine(GetRoles());
            StateHasChanged();
            //if (firstRender)
            //{
            //    var timer = new Timer(new TimerCallback(_ =>
            //    {
            //        uriHelper.NavigateTo(uriHelper.Uri, forceLoad: true);
            //    }), null, 20000, 20000);
            //}
            //Attempting to get nav menu to regenerate after login
        }


        public bool GetRoles()
        {
            CurrentUserRoles? i = new CurrentUserRoles();
            currentUserRole = CurrentUserRoles._role;
            StateHasChanged();
            if (currentUserRole != null)
            {
                temp = currentUserRole.First();
                if (currentUserRole.Contains("Admin"))
                {
                    Console.WriteLine("contains admin");
                }
                Console.WriteLine(temp);
                return true;
            }
            else
            {
                return false;   
            }
        }
    }
}
