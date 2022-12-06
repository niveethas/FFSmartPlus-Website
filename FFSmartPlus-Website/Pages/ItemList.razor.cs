using System;
using System.Runtime.CompilerServices;
using ClientAPI;
using Microsoft.AspNetCore.Components;
using MatBlazor;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using System.Text.Encodings.Web;

namespace FFSmartPlus_Website.Pages
{
    public partial class ItemList
    {
        public ICollection<UnitsDto> unitStock { get; set; }
        public ICollection<ItemDto> itemsInfo { get; set; }
        public Item newItemResponse { get; set; }
        
        public ItemDto? itemInfo {get;set;}
        public NewItemDto addItem { get; set; }
        public NewUnitDto addUnit { get; set; }

        public string inputItemName;
        public string inputUnitQuantity;
        public string inputSupplierID;
        public string inputUnitDesc;
        public string inputItemMinStock;
        public DateTime inputUnitExpDate;
        public string itemErrorMessage;
        private string inputID { get; set; }

        protected async override Task OnInitializedAsync()
        {
           
        }

        public async Task loadItem(string itemID)
        {
            string baseurl = "https://localhost:7041";
            var httpclient = new HttpClient();
            var _client = new FFSBackEnd(baseurl, httpclient);

            itemsInfo = await _client.ItemAllAsync();

            if (Regex.IsMatch(inputID, "^[0-9]*$")) {

                unitStock = await _client.UnitAsync(long.Parse(inputID));
                itemInfo = await _client.Item2Async(long.Parse(inputID));
            }
            // When the user searches an item, this function is called
            // All existing items wil be loaded in
            // The user's input is validated and passed to find the relevant item and unit info

        }
        public async Task deleteItem(long id)
        {
            string baseurl = "https://localhost:7041";
            var httpclient = new HttpClient();
            var _client = new FFSBackEnd(baseurl, httpclient);


            await _client.Item4Async(id);
            itemsInfo = await _client.ItemAllAsync();
            itemInfo = null;
            //function is called when the user presses the delete item button
            //the is of the item on display is passed in and the items are reloaded
            //itemInfo is set as null so the page resets

        }

        public async Task addNewItem(string name, string unitDesc, string minStock, string supplierID, string quantity, DateTime expiryDate )
        {
            string baseurl = "https://localhost:7041";
            var httpclient = new HttpClient();
            var _client = new FFSBackEnd(baseurl, httpclient);

            var newNID = new NewItemDto();
            var newAUD = new NewUnitDto();



            newNID.Name = name;
            newNID.UnitDesc = unitDesc;
            newNID.SupplierId = Int32.Parse(supplierID);
            newNID.MinimumStock = Convert.ToDouble(minStock);

            newAUD.ExpiryDate = expiryDate;
            newAUD.Quantity = Convert.ToDouble(quantity);

            newItemResponse = await _client.ItemAsync(newNID);
            Console.WriteLine(newItemResponse.Id);
            itemsInfo = await _client.ItemAllAsync();
            await _client.AddAsync(newItemResponse.Id,newAUD);
            
            //add new unit
            //add comments
            //clean up code - get rid of baseurl connections and rename variables
            //search by name?
            

        }

    }

}




