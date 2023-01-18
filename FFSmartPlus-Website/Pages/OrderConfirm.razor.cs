using ClientAPI;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace FFSmartPlus_Website.Pages
{
    public partial class OrderConfirm
    {
        [Inject]
        public FFSBackEnd? _client { get; set; }
        [Inject]
        protected IMatToaster Toaster { get; set; }

        public ICollection<SupplierOrderDto> orderItems;

        public bool checkedAllConfirm;

        public Dictionary<string, string> changedQuantities;
        public double newQuantity;
        public string additionSuccess;

        protected async override Task OnInitializedAsync()
        {
            orderItems = await _client.GenerateOrderAsync();
        }

        //waiting from nick for post method for the data collected

        void changeQuantity(long id, double quantity)
        {
            changedQuantities.Add("id", id.ToString());
            changedQuantities.Add("quantity", quantity.ToString());
            //use post to add new quanitty and id ?
        }

        async void confirmOrder()
        {
           // await _client.
        }

    }
}
