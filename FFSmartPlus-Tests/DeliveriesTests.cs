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
    public class DeliveriesTests
    {
        Deliveries deliveries;
        public DeliveriesTests()
        {
            deliveries = new Deliveries();
            deliveries._client = new FFSBackEnd("https://localhost:7041/", new HttpClient());

            var newLogin = new LoginModel();
            newLogin.Username = "admin1";
            newLogin.Password = "@Admin123"; // Password requires a symbol, capital letter and 3 numbers
            deliveries._client.LoginAsync(newLogin);
            var loginCode = deliveries._client.LoginAsync(newLogin);
            deliveries._client.Authorisation(loginCode.Result.Token);
        }

    }
}
