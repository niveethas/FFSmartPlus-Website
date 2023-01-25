using ClientAPI;
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


        protected async override Task OnInitializedAsync()
        {

            getSuppliers();
        }

        public async Task OnChangeSupplier(string supplier)
        {
            if (supplier != "-1")
                //do not want to consider the blank option in the drop down
            {
                selectedSupplierId = long.Parse(supplier);
            }
            //saves the supplier id of the supplier selected from the drop down menu
        }

        public async Task getSuppliers()
        {
            allSuppliers = await _client.SupplierAllAsync();
            getSuppliersToList();
        }

        public List<SupplierDto> getSuppliersToList()
        {
            return allSuppliers.ToList();
        }


        public async Task getSupplierById()
        {
           selectedSupplier  = await _client.Supplier2Async(selectedSupplierId);
        }

        public async Task deleteSupplier(long id)
        {
            await _client.Supplier4Async(id);
            getSuppliers();
            //resets the dropdown by recalling the method to get all suppliers.
        }

        public async Task changeSupplierDetails(long id, string address, string email, string name)
        {
            SupplierDto modifiedSupplier = new SupplierDto();
            modifiedSupplier.Address = address;
            modifiedSupplier.Email = email;
            modifiedSupplier.Name = name;

            //add if null checks, take original values. 
            await _client.Supplier3Async(id, modifiedSupplier);
        }

        public void changeSupplierOption()
        {
            changeSupplierCheck = "True";
        }

        public void supplierOptionStatus()
        {
            changeSupplierCheck = "";
        }


    }
}
