global using Microsoft.VisualStudio.TestTools.UnitTesting;
global using FFSmartPlus_Website.Pages;
using Microsoft.AspNetCore.Components;
using ClientAPI;

internal class TestConsts {
    public const string TRUE_STR = "True";
    public const string FALSE_STR = "False";
}

internal class SetupClient
{
    private const string UN_ADMIN = "Admin2";
    private const string PWD_ADMIN = "@Admin123";

    public FFSBackEnd SignInAdmin()
    {
        FFSBackEnd client = new FFSBackEnd("https://localhost:7041/", new HttpClient());

        var newLogin = new LoginModel();
        newLogin.Username = UN_ADMIN;
        newLogin.Password = PWD_ADMIN; // Password requires a symbol, capital letter and 3 numbers
        client.LoginAsync(newLogin);
        var loginCode = client.LoginAsync(newLogin);
        client.Authorisation(loginCode.Result.Token);

        return client;
    }
}
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