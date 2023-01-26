using System;
using System.Runtime.CompilerServices;
using ClientAPI;
using Microsoft.AspNetCore.Components;
using MatBlazor;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using System.Text.Encodings.Web;
using System.IdentityModel.Tokens.Jwt;

namespace FFSmartPlus_Website.Pages
{
    public partial class ItemList
    {
        [Inject]
        protected IMatToaster Toaster { get; set; }
        public ICollection<UnitsDto> unitStock { get; set; }
        public ICollection<ItemDto> itemsInfo { get; set; }
        public ItemDto newItemResponse;
        
        public ItemDto? itemInfo {get;set;}
        public NewItemDto addItem { get; set; }
        public NewUnitDto addUnit { get; set; }

        public string inputItemName;
        public string inputUnitQuantity;                                              
        public string inputSupplierID;
        public string inputUnitDesc;
        public string inputItemMinStock;
        public string inputItemDesiredStock;
        public DateTime inputUnitExpDate;
        public string itemErrorMessage;
        public long returnedId;
        public string inputItemIdE;
        public string inputUnitQuantityE;
        public DateTime inputUnitExpDateE;

        public bool itemAdditionSuccess = false;
        public string stockAdditionSuccess = "";

        public List<string> currentRoles;

        private string inputID { get; set; }

        [Inject]
        public FFSBackEnd? _client { get; set; }



        protected async override Task OnInitializedAsync()
        {
            //CurrentUserRoles i = new CurrentUserRoles();
            //currentRoles = CurrentUserRoles._role;

        }

        public async Task GetRoles()
        {
           
        }

        public async Task loadItem(string itemID)
        {
            //itemsInfo = await _client.ItemAllAsync();

            try
            {
                if (Regex.IsMatch(itemID, "^[0-9]*$"))
                {
                    unitStock = await _client.UnitAsync(long.Parse(itemID));
                    itemInfo = await _client.Item2Async(long.Parse(itemID));
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            // When the user searches an item, this function is called
            // All existing items wil be loaded in
            // The user's input is validated and passed to find the relevant item and unit info

        }
        public async Task deleteItem(long id)
        {
            try
            {
                await _client.Item4Async(id);
                itemsInfo = await _client.ItemAllAsync();
                itemInfo = null;
                //function is called when the user presses the delete item button
                //the is of the item on display is passed in and the items are reloaded
                //itemInfo is set as null so the page resets
            }
            catch(Exception e)
            {
                Console.WriteLine(e);                
            }
        }

        public async Task addNewItem(string name, string unitDesc, string minStock, string supplierID, string desiredStock )
        {
            try
            {
                var newNID = new NewItemDto();

                //instantiates a NewItemDto class and populates it with user input
                newNID.Name = name;
                newNID.UnitDesc = unitDesc;
                newNID.SupplierId = Int32.Parse(supplierID);
                newNID.MinimumStock = Convert.ToDouble(minStock);
                newNID.DesiredStock = Convert.ToDouble(desiredStock);

                var newItemResponse = await _client.ItemAsync(newNID);

                //checks an id has been returned before showing the stock information
                if (newItemResponse.Id != null)
                {
                    itemAdditionSuccess = true;
                    returnedId = newItemResponse.Id;
                }
            }
            catch(Exception e)
            {
                itemAdditionSuccess = false;
            }
           
        }

        public async Task addStock (string quantity, DateTime expiryDate)
        {
            stockAdditionSuccess = "";

            try
            {
                var newAUD = new NewUnitDto();
                newAUD.ExpiryDate = expiryDate;
                newAUD.Quantity = Convert.ToDouble(quantity);

                //instantiates a NewUnitDto class and populates it with user input
                //using the returned Id from the post method and send object with expirydate, quantity
                await _client.AddAsync(returnedId, newAUD);

                stockAdditionSuccess = "True";
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                stockAdditionSuccess = "False";
            }
        }

        public async Task addStockExisting (string quantity, DateTime expiryDate, string itemId)
        {
            try
            {
                var newAUD = new NewUnitDto();
                newAUD.ExpiryDate = expiryDate;
                newAUD.Quantity = Convert.ToDouble(quantity);
                var itemID = long.Parse(itemId);
                //instantiates a NewUnitDto class and populates it with user input
                //using the returned Id from the post method and send object with expirydate, quantity
                await _client.AddAsync(itemID, newAUD);

            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }



    }

}




