﻿using ClientAPI;
using MatBlazor;
using Microsoft.AspNetCore.Components;

namespace FFSmartPlus_Website.Pages
{
    public partial class Suppliers
    {
        
        [Inject]
        public FFSBackEnd? _client { get; set; }
        [Inject]
        protected IMatToaster Toaster { get; set; }

        public ICollection<SupplierDto> allSuppliers = new List<SupplierDto>();
        public SupplierDto selectedSupplier = new SupplierDto();

        public long selectedSupplierId;
        public string changeSupplierCheck;
        public string inputName;
        public string inputEmail;
        public string inputAddress;
        public string inputNameNew;
        public string inputEmailNew;
        public string inputAddressNew;

        public string supplierAdditionSuccess;
        public string supplierModifySuccess;
        public string supplierDeleteSuccess;

        public List<string> currentUserRole;

        protected async override Task OnInitializedAsync()
        {
            CurrentUserRoles? i = new CurrentUserRoles();
            currentUserRole = CurrentUserRoles._role;
            StateHasChanged();
            allSuppliers = await _client.SupplierAllAsync();
            //get all the suppliers on load of page
        }

        public async Task OnChangeSupplier(string supplier)
        {
            try
            {
                if (supplier != "-1")
                //do not want to consider the blank option in the drop down
                {
                    selectedSupplierId = Convert.ToInt64(supplier);
                }
                await getSupplierById();
                //saves the supplier id of the supplier selected from the drop down menu
            }
            catch(Exception e)
            {
                selectedSupplierId = 0;
                Console.WriteLine(e);
            }
        }

        

        public List<SupplierDto> getSuppliersToList()
        {
            return allSuppliers.ToList();
            //so the frontend display can make use of this to display suppliers
        }


        public async Task getSupplierById()
        {
           selectedSupplier  = await _client.Supplier2Async(selectedSupplierId);
            //get the supplier details by id and show the details
        }

        public async Task deleteSupplier(long id)
        {
            try
            {
                await _client.Supplier4Async(id);
                supplierDeleteSuccess = "True";

            }
            catch
            {
                supplierDeleteSuccess = "False";
            }
            //delete a supplier using the id given
            
        }

        public async Task changeSupplierDetails(long id, string? address, string? email, string? name)
        {
            SupplierDto modifiedSupplier = new SupplierDto();
            modifiedSupplier.Id = id;
            if (address != null)
            {
                modifiedSupplier.Address = address;
            }
            else
            {
                modifiedSupplier.Address = selectedSupplier.Address;
            }
            if (email != null)
            {
                modifiedSupplier.Email = email;

            }
            else
            {
                modifiedSupplier.Email = selectedSupplier.Email;

            }
            if (name != null)
            {
                modifiedSupplier.Name = name;

            }
            else
            {
                modifiedSupplier.Name = selectedSupplier.Name;

            }

            //if null take original values. 
            //pass back id of supplier and changed information dto
            try
            {
                await _client.Supplier3Async(id, modifiedSupplier);
                supplierModifySuccess = "True";

            }
            catch 
            {
                supplierModifySuccess = "False";
            }
        }

        public async Task AddNewSupplier (string address, string email, string name)
        {
            NewSupplierDto newSupplier = new NewSupplierDto();
            newSupplier.Address = address;
            newSupplier.Email = email;
            newSupplier.Name = name;
            try
            {
                await _client.SupplierAsync(newSupplier);
                supplierAdditionSuccess = "True";
            }
            catch
            {
                supplierAdditionSuccess= "False";
            }
            //take user input values and send information to create new supplier.
        }

        public void changeSupplierOption()
        {
            changeSupplierCheck = "True";
            //to change view
        }

        public void supplierOptionStatus()
        {
            changeSupplierCheck = "";
            //to reset view
        }

        public void toastStatus()
        {
            supplierAdditionSuccess = "";
            supplierDeleteSuccess = "";
            supplierModifySuccess = "";
        }


    }
}
