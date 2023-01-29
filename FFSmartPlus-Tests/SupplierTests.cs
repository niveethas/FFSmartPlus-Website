using ClientAPI;
using FFSmartPlus_Website.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFSmartPlus_Tests
{
    [TestClass]
    public class SupplierTests
    {
        Suppliers suppliers;

        public SupplierTests()
        {
            suppliers = new Suppliers();
            suppliers._client = new FFSBackEnd("https://localhost:7041/", new HttpClient());

            var newLogin = new LoginModel();
            newLogin.Username = "admin1";
            newLogin.Password = "@Admin123"; // Password requires a symbol, capital letter and 3 numbers
            suppliers._client.LoginAsync(newLogin);
            var loginCode = suppliers._client.LoginAsync(newLogin);
            suppliers._client.Authorisation(loginCode.Result.Token);
        }

    }
}
