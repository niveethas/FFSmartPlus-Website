global using Microsoft.VisualStudio.TestTools.UnitTesting;
global using FFSmartPlus_Website.Pages;
using Microsoft.AspNetCore.Components;

internal class TestNav : NavigationManager
{
    public TestNav()
    {
        Initialize("https://localhost:7143/", "https://localhost:7143/");
    }

    protected override void NavigateToCore(string uri, bool forceLoad)
    {
        NotifyLocationChanged(false);
    }
}