using System;
using ClientAPI;
using Microsoft.AspNetCore.Components;


namespace FFSmartPlus_Website.Pages
{
    public partial class ItemList
    {
        public ICollection<ItemDto> Items { get; set; }
        protected async override Task OnInitializedAsync()
        {
            string baseurl = "https://localhost:7041";
            var httpclient = new HttpClient();
            var _client = new FFSBackEnd(baseurl, httpclient);
            Items = await _client.ItemAllAsync();
            //The .ItemAllAsync referes to the different API post/get/put/delete commands (they're named confusingly)

        }
    }
}




