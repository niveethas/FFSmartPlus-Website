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
        OrderConfirm _orderConfirm;

        long validId = 1;

        double originalQuantity = 0;


        public OrderConfirmTests()
        {
            _orderConfirm = new OrderConfirm();
            _orderConfirm._client = new SetupClient().SignInAdmin();

        }

        #region changeQuantity
        //changeQuantity(long id, string quantity)
        [TestMethod]
        public async Task T1_ChangeQuantity_Valid_Integer()
        {
            _orderConfirm.orderItems = await _orderConfirm._client.GenerateOrderAsync();
            validId = _orderConfirm.orderItems.ElementAt(0).Orders.ElementAt(0).Id;
            originalQuantity = _orderConfirm.orderItems.ElementAt(0).Orders.ElementAt(0).OrderQuantity;
            
            string validQuantity = "5";
            await _orderConfirm.changeQuantity(validId, validQuantity);

            Assert.AreEqual(TestConsts.TRUE_STR, _orderConfirm.additionSuccess);

            //originalQuantity += 5;

            //Assert.AreEqual(originalQuantity, orderConfirm.orderItems.ElementAt(0).Orders.ElementAt(0).OrderQuantity);
        }

        [TestMethod]
        public async Task T2_ChangeQuantity_Valid_Decimal()
        {
            string validQuantity = "0.5";
            await _orderConfirm.changeQuantity(validId, validQuantity);

            Assert.AreEqual(TestConsts.TRUE_STR, _orderConfirm.additionSuccess);

            //originalQuantity += 0.5;
            //Assert.AreEqual(originalQuantity, orderConfirm.orderItems.ElementAt(0).Orders.ElementAt(0).OrderQuantity);

        }

        [TestMethod]
        public async Task T3_ChangeQuantity_Invalid_InvalidId()
        {
            string validQuantity = "5";
            long invalidId = 500;
            await _orderConfirm.changeQuantity(invalidId, validQuantity);

            Assert.AreEqual(TestConsts.FALSE_STR, _orderConfirm.additionSuccess);
        }

        [TestMethod]
        public async Task T3_ChangeQuantity_Invalid_InvalidQuantity_StringChars()
        {
            string invalidQuantity = "abc";
            await _orderConfirm.changeQuantity(validId, invalidQuantity);

            Assert.AreEqual(TestConsts.FALSE_STR, _orderConfirm.additionSuccess);
        }
        #endregion
    }
}
