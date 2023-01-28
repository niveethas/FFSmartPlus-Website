using ClientAPI;
using MatBlazor;
using Microsoft.AspNetCore.Components;

namespace FFSmartPlus_Website.Pages
{
    public partial class Deliveries
    {
        

        [Inject]
        public FFSBackEnd? _client { get; set; }
        [Inject]
        protected IMatToaster Toaster { get; set; }

        public CurrentUserRoles currentUser = new CurrentUserRoles();
        public DateTime inputOrderDate;
        public DateTime inputExpDate;
        public string inputSupplierId;
        public ActiveOrdersDto deliveriesOnDate;
        public OrderConfirmationDTO orderConfirmation = new OrderConfirmationDTO();
        public long currentOrderId;
        public double currentOrderQuant;
        public async Task getDeliveriesOnDate(string id, DateTime date)
        {
            long supplierId = long.Parse(id);
            deliveriesOnDate = await _client.DeliveryAsync(supplierId, date);
            //calls the get method to retrieve a list orders within specified supplier and date
        }

        public async Task confirmOrder(string id, string quantity, DateTime date )
        {
            //this will change
            NewUnitDto newUnit = new NewUnitDto();
            newUnit.Quantity = Int32.Parse(quantity);
            newUnit.ExpiryDate = date;
            orderConfirmation.OrderLogId = long.Parse(id);
            orderConfirmation.UnitDeliver = newUnit;
            await _client.ConfirmAsync(orderConfirmation);
        }

        public async Task ConfirmCurrentDelivery(long id, double quantity, DateTime expDate)
        {
            //not sure if this is right

            OrderConfirmationDTO orderConfirmation = new OrderConfirmationDTO();
            orderConfirmation.OrderLogId = id; //think this is wrong
            NewUnitDto newUnit = new NewUnitDto();
            newUnit.Quantity = quantity;
            newUnit.ExpiryDate = expDate;
            orderConfirmation.UnitDeliver = newUnit;
            await _client.ConfirmAsync(orderConfirmation);
        }
    }
}
