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
        public ICollection<ItemDto> itemsInfo = new List<ItemDto>();
        public ItemDto newItemResponse;
        
        public ItemDto? itemInfo {get;set;}
        public NewItemDto addItem { get; set; }
        public NewUnitDto addUnit { get; set; }

        public string selectedItemId;
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
        public string inputItemIdR;
        public string inputUnitQuantityR;
        public string? inputItemNameM;
        public string? inputSupplierIDM;
        public string? inputUnitDescM;
        public string? inputItemMinStockM;
        public string? inputItemDesiredStockM;

        public string itemAdditionSuccess = "";
        public string stockModifySuccess = "";
        public string itemFound = "";
        public string showItemModifyContent = "";
        public string itemModifySuccess = "";

        public List<string> currentRoles;

        private string inputID { get; set; }

        [Inject]
        public FFSBackEnd? _client { get; set; }

        public List<string> currentUserRole;


        protected async override Task OnInitializedAsync()
        {
            CurrentUserRoles? i = new CurrentUserRoles();
            currentUserRole = CurrentUserRoles._role;
            StateHasChanged();

            itemsInfo = await _client.ItemAllAsync();

        }

        public List<ItemDto> itemsToList()
        {
            return itemsInfo.ToList();
        }

        public void onChangeItem(string id)
        {
            selectedItemId = id;
            loadItem(selectedItemId);
        }

        public async Task GetRoles()
        {
           
        }


        public async Task loadItem(string itemID)
        {
            
            if (Regex.IsMatch(itemID, "^[0-9]*$")) {
                try
                {
                    itemInfo = await _client.Item2Async(long.Parse(itemID));
                    StateHasChanged();
                    try
                    {
                        await loadStock(itemID);
                    }
                    catch
                    {
                        itemFound = "False";
                    }
                   
                }catch
                {
                    itemFound = "False";
                }
                

                
            }
            // When the user searches an item, this function is called
            // All existing items wil be loaded in
            // The user's input is validated and passed to find the relevant item and unit info

        }

        public async Task loadStock(string itemID)
        {
            unitStock = await _client.UnitAsync(long.Parse(itemID));
            StateHasChanged();
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
                    itemAdditionSuccess = "True";
                    returnedId = newItemResponse.Id;
                }
            }
            catch(Exception e)
            {
                itemAdditionSuccess = "False";
            }
           
        }

        public async Task addStock (string quantity, DateTime expiryDate)
        {

            try
            {
                var newAUD = new NewUnitDto();
                newAUD.ExpiryDate = expiryDate;
                newAUD.Quantity = Convert.ToDouble(quantity);

                //instantiates a NewUnitDto class and populates it with user input
                //using the returned Id from the post method and send object with expirydate, quantity
                await _client.AddAsync(returnedId, newAUD);

                stockModifySuccess = "True";
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                stockModifySuccess = "False";
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
                stockModifySuccess = "True";

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                stockModifySuccess = "False";

            }
        }

        public async Task removeStock(string quantity, string itemId)
        {

            try
            {
                var idLong = Int64.Parse(itemId);
                var quantDouble = Convert.ToDouble(quantity);

                //take the users input of id and delete the 
                await _client.RemoveAsync(idLong, quantDouble);
                stockModifySuccess = "True";
            }
            catch (ApiException<ValidationResult> ex)
            {
                //returning the error messages into a list for dev
                var errorMessages = ex.Result.Errors.Select(i => i.ErrorMessage).ToList();
                stockModifySuccess = "False";
            }
            catch (Exception e)
            {
                stockModifySuccess = "False";
                //generic catch to ensure the user is informed of failure
            }


        }

        public async Task modifyItem(string? name, string? minStock, string? desStock, string? supplierId, string? unitDesc )
        {
            try
            {
                ItemDto modifyItem = new ItemDto();
                if (name == null)
                {
                    modifyItem.Name = itemInfo.Name;

                }
                else
                {
                    modifyItem.Name = name;
                }
                if (minStock == null)
                {
                    modifyItem.MinimumStock = itemInfo.MinimumStock;
                }
                else
                {
                    modifyItem.MinimumStock = Convert.ToDouble(minStock);
                }
                if (desStock == null)
                {
                    modifyItem.DesiredStock = itemInfo.DesiredStock;
                }
                else
                {
                    modifyItem.DesiredStock = Convert.ToDouble(desStock);
                }
                if (supplierId == null)
                {
                    modifyItem.SupplierId = itemInfo.SupplierId;
                }
                else
                {
                    modifyItem.SupplierId = long.Parse(supplierId);
                }
                if (unitDesc == null)
                {
                    modifyItem.UnitDesc = itemInfo.UnitDesc;
                }
                else
                {
                    modifyItem.UnitDesc = unitDesc;
                }

                modifyItem.Id = itemInfo.Id;

                try {

                    await _client.Item3Async(itemInfo.Id, modifyItem);
                    StateHasChanged();
                    await loadItem(itemInfo.Id.ToString());
                    itemModifySuccess = "True";
                }
                catch (ApiException<ValidationResult> ex)
                {
                    itemModifySuccess = "False";
                    var errorMessages = ex.Result.Errors.Select(i => i.ErrorMessage).ToList();
                    foreach (var x in errorMessages)
                    {
                        Console.WriteLine(errorMessages);
                    }
                    StateHasChanged();
                }
            }
            catch
            {
                itemModifySuccess = "False";

            }
        }
        public void modifyItemOption()
        {
            showItemModifyContent = "True";
        }

        public void toastStatus()
        {
            stockModifySuccess = "";
            itemModifySuccess = "";
            itemAdditionSuccess = "";
        }

        public void itemFoundStatus()
        {
            itemFound = "";
        }

        public void contentStatus()
        {
            showItemModifyContent = "";
        }
    }

}




