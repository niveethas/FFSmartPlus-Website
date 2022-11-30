using System;
using System.Runtime.CompilerServices;
using ClientAPI;
using Microsoft.AspNetCore.Components;
using MatBlazor;


namespace FFSmartPlus_Website.Pages
{
    public partial class ItemList
    {
        public ICollection<UnitsDto> unitStock { get; set; }
        public ICollection<ItemDto> itemsInfo { get; set; }

        public ItemDto? itemInfo {get;set;}
        public NewItemDto addItem { get; set; }
        public ICollection<NewUnitDto> addUnit { get; set; }
        public string inputItemName;
        public string inputItemQuantity;
        public string inputSupplierID;
        public string inputUnitDesc;
        public string inputItemMinStock;
        public string inputUnitExpDate;


        private string inputID { get; set; }

        protected async override Task OnInitializedAsync()
        {
            string baseurl = "https://localhost:7041";
            var httpclient = new HttpClient();
            var _client = new FFSBackEnd(baseurl, httpclient);
            unitStock = await _client.UnitAsync(long.Parse(inputID));
            //itemsInfo = await _client.ItemAllAsync();
        }

        public async Task loadItem(string itemID)
        {
            string baseurl = "https://localhost:7041";
            var httpclient = new HttpClient();
            var _client = new FFSBackEnd(baseurl, httpclient);
            itemsInfo = await _client.ItemAllAsync();
            unitStock = await _client.UnitAsync(long.Parse(inputID));
            itemInfo = await _client.Item2Async(long.Parse(inputID));
            Console.WriteLine(itemInfo.Name);
        }
        public async Task deleteItem(long id)
        {
            string baseurl = "https://localhost:7041";
            var httpclient = new HttpClient();
            var _client = new FFSBackEnd(baseurl, httpclient);
            Console.WriteLine(id);
            await _client.Item4Async(id);
            itemsInfo = await _client.ItemAllAsync();
            itemInfo = null;

        }

        public async Task addNewItem(string name, string unitDesc, string minStock, string supplierID )
        {
            string baseurl = "https://localhost:7041";
            var httpclient = new HttpClient();
            var _client = new FFSBackEnd(baseurl, httpclient);

            addItem.Name = name;
            addItem.UnitDesc = unitDesc;
            addItem.SupplierId = Int32.Parse(supplierID);
            addItem.MinimumStock = Convert.ToDouble(minStock);

            await _client.ItemAsync(addItem);

        }

    }

}




