using FFSmartPlus_Website.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Internal;

namespace FFSmartPlus_Website.Shared
{
    public partial class NavMenu
    {
         public void signOutRole()
        {
            CurrentUserRoles? i = new CurrentUserRoles();
            CurrentUserRoles._role = null;
        }
    }
}
