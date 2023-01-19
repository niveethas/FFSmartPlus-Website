using FFSmartPlus_Website.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Internal;

namespace FFSmartPlus_Website.Shared
{
    public partial class NavMenu
{
        public List<string> currentUserRole;

        [Inject]
        public NavigationManager uriHelper { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (!firstRender)
            {
                var timer = new Timer(new TimerCallback(_ =>
                {
                    uriHelper.NavigateTo(uriHelper.Uri, forceLoad: true);
                }), null, 2000, 2000);
            }
            //Attempting to get nav menu to regenerate after login
        }
        public async Task GetRoles()
        {
            CurrentUserRoles i = new CurrentUserRoles();
            currentUserRole = CurrentUserRoles._role;
            
        }
    }
}
