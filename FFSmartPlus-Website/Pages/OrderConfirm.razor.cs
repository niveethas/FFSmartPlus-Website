using ClientAPI;
using MatBlazor;
using Microsoft.AspNetCore.Components;

namespace FFSmartPlus_Website.Pages
{
    public partial class OrderConfirm
    {
        [Inject]
        public FFSBackEnd? _client { get; set; }
        [Inject]
        protected IMatToaster Toaster { get; set; }

        public ICollection<SupplierOrderDto> orderItems;

        protected async override Task OnInitializedAsync()
        {
            orderItems = await _client.GenerateOrderAsync();
        }

    }
}
