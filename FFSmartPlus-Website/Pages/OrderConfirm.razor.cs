using ClientAPI;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using System.IdentityModel.Tokens.Jwt;
using System.Diagnostics.CodeAnalysis;

namespace FFSmartPlus_Website.Pages
{
    public partial class OrderConfirm
    {
        [Inject]
        public FFSBackEnd? _client { get; set; }
        [Inject]
        protected IMatToaster Toaster { get; set; }

        public ICollection<SupplierOrderDto> orderItems = new List<SupplierOrderDto>();


        public string newQuantity;
        public string additionSuccess;

        protected async override Task OnInitializedAsync()
        {
            orderItems = await _client.GenerateOrderAsync();
        }


        public async Task changeQuantity(long id, string quantity)
        {
            ItemRequestDto changedQuantityRequest = new ItemRequestDto();
            OrderRequestDto changedQuantityOrder = new OrderRequestDto();
            changedQuantityRequest.Quantity = Convert.ToDouble(quantity);
            changedQuantityRequest.Id = id;
            ICollection<ItemRequestDto> itemRequests = new List<ItemRequestDto>();
            try
            {
                itemRequests.Add(changedQuantityRequest);
                changedQuantityOrder.Items = itemRequests;
                var cobIDResponse = await _client.ConfirmOrderByIDsAsync(changedQuantityOrder);
                if (cobIDResponse)
                {
                    additionSuccess = "True";
                    orderItems = await _client.GenerateOrderAsync();

                }
                else
                {
                    additionSuccess = "False";

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);  
            }
        }


        public async Task ToasterStatus()
        {
           additionSuccess = "";

        }

    }
}
