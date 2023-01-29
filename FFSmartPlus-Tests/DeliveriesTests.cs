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
        // TODO: once deliveries page is functional

        Deliveries _deliveries;
        public DeliveriesTests()
        {
            _deliveries = new Deliveries();
            _deliveries._client = new SetupClient().SignInAdmin();
        }

    }
}
