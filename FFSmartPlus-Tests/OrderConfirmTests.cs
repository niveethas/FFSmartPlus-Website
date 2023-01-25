using ClientAPI;
using FFSmartPlus_Website;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFSmartPlus_Tests
{
    [TestClass]
    public class OrderConfirmTests
    {
        OrderConfirm orderConfirm;

        long validId = 1;

        double originalQuantity = 0;


        public OrderConfirmTests()
        {
            orderConfirm = new OrderConfirm();
            orderConfirm._client = new FFSBackEnd("https://localhost:7041/", new HttpClient());

            var newLogin = new LoginModel();
            newLogin.Username = "admin1";
            newLogin.Password = "@Admin123"; // Password requires a symbol, capital letter and 3 numbers
            orderConfirm._client.LoginAsync(newLogin);
            var loginCode = orderConfirm._client.LoginAsync(newLogin);
            orderConfirm._client.AddAuth(loginCode.Result.Token);

        }


        //changeQuantity(long id, string quantity)
        [TestMethod]

        public async Task T1_ChangeQuantity_Valid_Integer()
        {
            orderConfirm.orderItems = await orderConfirm._client.GenerateOrderAsync();
            validId = orderConfirm.orderItems.ElementAt(0).Orders.ElementAt(0).Id;
            originalQuantity = orderConfirm.orderItems.ElementAt(0).Orders.ElementAt(0).OrderQuantity;
            
            string validQuantity = "5";
            await orderConfirm.changeQuantity(validId, validQuantity);

            Assert.AreEqual(TestConsts.TRUE_STR, orderConfirm.additionSuccess);

            originalQuantity += 5;

            Assert.AreEqual(originalQuantity, orderConfirm.orderItems.ElementAt(0).Orders.ElementAt(0).OrderQuantity);
        }

        [TestMethod]

        public async Task T2_ChangeQuantity_Valid_Decimal()
        {
            string validQuantity = "0.5";
            await orderConfirm.changeQuantity(validId, validQuantity);

            Assert.AreEqual(TestConsts.TRUE_STR, orderConfirm.additionSuccess);

            originalQuantity += 0.5;
            Assert.AreEqual(originalQuantity, orderConfirm.orderItems.ElementAt(0).Orders.ElementAt(0).OrderQuantity);

        }

        [TestMethod]

        public async Task T3_ChangeQuantity_Invalid_InvalidId()
        {
            string validQuantity = "5";
            long invalidId = 500;
            await orderConfirm.changeQuantity(invalidId, validQuantity);

            Assert.AreEqual(TestConsts.FALSE_STR, orderConfirm.additionSuccess);
        }

        [TestMethod]

        public async Task T3_ChangeQuantity_Invalid_InvalidQuantity_StringChars()
        {
            string invalidQuantity = "abc";
            await orderConfirm.changeQuantity(validId, invalidQuantity);

            Assert.AreEqual(TestConsts.FALSE_STR, orderConfirm.additionSuccess);
        }

    }
}
