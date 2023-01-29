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
        public async Task TA1_ChangeQuantity_Valid_Integer()
        {
            // change order quantity - valid - integer value
            // pre-loading the order
            _orderConfirm.orderItems = await _orderConfirm._client.GenerateOrderAsync();
            validId = _orderConfirm.orderItems.ElementAt(0).Orders.ElementAt(0).Id;
            originalQuantity = _orderConfirm.orderItems.ElementAt(0).Orders.ElementAt(0).OrderQuantity;
            
            string validQuantity = "5";
            await _orderConfirm.changeQuantity(validId, validQuantity);

            Assert.AreEqual(TestConsts.TRUE_STR, _orderConfirm.additionSuccess);
        }

        [TestMethod]
        public async Task TA2_ChangeQuantity_Valid_Decimal()
        {
            // change order quantity - decimal
            string validQuantity = "0.5";
            await _orderConfirm.changeQuantity(validId, validQuantity);

            Assert.AreEqual(TestConsts.TRUE_STR, _orderConfirm.additionSuccess);
        }

        [TestMethod]
        public async Task TA3_ChangeQuantity_Invalid_InvalidId()
        {
            // change order quantity - invalid item id
            string validQuantity = "5";
            long invalidId = 500;
            await _orderConfirm.changeQuantity(invalidId, validQuantity);

            Assert.AreEqual(TestConsts.FALSE_STR, _orderConfirm.additionSuccess);
        }

        [TestMethod]
        public async Task TA4_ChangeQuantity_Invalid_InvalidQuantity_StringChars()
        {
            // change order quantity - invalid quantitiy - non numerical
            string invalidQuantity = "abc";
            await _orderConfirm.changeQuantity(validId, invalidQuantity);

            Assert.AreEqual(TestConsts.FALSE_STR, _orderConfirm.additionSuccess);
        }
        #endregion
    }
}
