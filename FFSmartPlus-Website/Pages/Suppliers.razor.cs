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

        ICollection<SupplierDto> allSuppliers = new List<SupplierDto>();
        SupplierDto selectedSupplier = new SupplierDto();

        public long selectedSupplierId;
        public string changeSupplierCheck;
        public string inputName;
        public string inputEmail;
        public string inputAddress;
        public string inputNameNew;
        public string inputEmailNew;
        public string inputAddressNew;


        protected async override Task OnInitializedAsync()
        {
            allSuppliers = await _client.SupplierAllAsync();
            //get all the suppliers on load of page
        }

        public async Task OnChangeSupplier(string supplier)
        {
            if (supplier != "-1")
                //do not want to consider the blank option in the drop down
            {
                selectedSupplierId = Convert.ToInt64(supplier);
            }
            await getSupplierById();
            //saves the supplier id of the supplier selected from the drop down menu
        }

        public async Task getSuppliers()
        {
            var t = await _client.SupplierAllAsync();
            //calls all suppliers using get method and calls the list function
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
            await _client.Supplier4Async(id);
            //delete a supplier using the id given
            getSuppliers();
            //resets the dropdown by recalling the method to get all suppliers.
        }

        public async Task changeSupplierDetails(long id, string? address, string? email, string? name)
        {
            SupplierDto modifiedSupplier = new SupplierDto();
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
                modifiedSupplier.Email = selectedSupplier.Email;

            }

            //if null take original values. 
            //pass back id of supplier and changed information dto
            await _client.Supplier3Async(id, modifiedSupplier);
            supplierOptionStatus();
        }

        public async Task AddNewSupplier (string address, string email, string name)
        {
            NewSupplierDto newSupplier = new NewSupplierDto();
            newSupplier.Address = address;
            newSupplier.Email = email;
            newSupplier.Name = name;
            await _client.SupplierAsync(newSupplier);
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


    }
}