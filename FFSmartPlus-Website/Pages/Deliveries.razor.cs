using ClientAPI;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using ServiceStack;

namespace FFSmartPlus_Website.Pages
{
    public partial class Deliveries
    {


        [Inject]
        public FFSBackEnd? _client { get; set; }
        [Inject]
        protected IMatToaster Toaster { get; set; }

        public DateTime inputOrderDate;
        public DateTime inputExpDate;
        public string inputSupplierId;
        public ActiveOrdersDto deliveriesOnDate = new ActiveOrdersDto();
        public OrderConfirmationDTO orderConfirmation = new OrderConfirmationDTO();
        public long currentOrderId;
        public double currentOrderQuant;
        public string currentOrderName;
        public string currentOrderDescription;
        public string findDelivery;
        public string confirmationSuccess;
        public string? deliveredQuant;
        public List<string> currentUserRole;

        protected async override Task OnInitializedAsync()
        {
            CurrentUserRoles? i = new CurrentUserRoles();
            currentUserRole = CurrentUserRoles._role;
            StateHasChanged();

        }

        public async Task getDeliveriesOnDate(string id, DateTime date)
        {
            try
            {
                long supplierId = long.Parse(id);
                deliveriesOnDate = await _client.DeliveryAsync(supplierId, date);
                StateHasChanged();
                findDelivery = "True";
                //calls the get method to retrieve a list orders within specified supplier and date
            } catch
            {
                findDelivery = "False";
            }
        }

        public List<ActiveOrderDto> getDeliveriesToList()
        {
            return deliveriesOnDate.Orders.ToList();
            //returns the deliveries retrieved into a list for dropdown menu
        }

        public void onChangeOrder(string id)
        {
            if (!id.Equals("-1"))
            {
                currentOrderId = Int64.Parse(id);
                var orders = deliveriesOnDate.Orders.ToList();
                //find the order quantity based on the id, which was retrieved using the drop down menu
                for (int i = 0; i < orders.Count(); i++)
                {
                    if (currentOrderId == orders[i].Id)
                    {
                        currentOrderQuant = orders[i].QuantityOrdered;
                        currentOrderName = orders[i].Name;
                        currentOrderDescription = orders[i].UnitDescription;

                    }
                }
            }
        }


        public async Task ConfirmCurrentDelivery(DateTime expDate, string quantity)
        {
            try
            {
                OrderConfirmationDTO orderConfirmation = new OrderConfirmationDTO();
                orderConfirmation.OrderLogId = currentOrderId;
                NewUnitDto newUnit = new NewUnitDto();
                if (quantity.IsEmpty())
                {
                    newUnit.Quantity = currentOrderQuant;

                }else
                {
                    newUnit.Quantity = Int32.Parse(quantity);  
                }
                newUnit.ExpiryDate = expDate;
                orderConfirmation.UnitDeliver = newUnit;
                await _client.ConfirmAsync(orderConfirmation);
                confirmationSuccess = "True";
                currentOrderId = 0;
                StateHasChanged();
            }
            catch
            {
                confirmationSuccess = "False";
                StateHasChanged();
            }
        }

        public void toastStatus()
        {
            confirmationSuccess = "";
            findDelivery = "";
        }

}
}
