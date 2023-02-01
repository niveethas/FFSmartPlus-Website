using ClientAPI;
using MatBlazor;
using Microsoft.AspNetCore.Components;

namespace FFSmartPlus_Website.Pages
{

    public partial class AdminAudit
{
        [Inject]
        public FFSBackEnd? _client { get; set; }
        [Inject]
        protected IMatToaster Toaster { get; set; }

        public ICollection<UnitListDto> allExpStock;
        public string inputHistory;

        public string deletionSuccess;
        public string auditSuccess;

        public List<string> currentUserRole;

        protected async override Task OnInitializedAsync()
        {
            CurrentUserRoles? i = new CurrentUserRoles();
            currentUserRole = CurrentUserRoles._role;
            StateHasChanged();
            allExpStock = await _client.ExpiryAsync();
            //get all the suppliers on load of page

        }


        public List<UnitListDto> expStockToList()
        {
            return allExpStock.ToList();    
        }

        public async Task deleteExpStock()
        {
            try
            {
                await _client.EndOfDayAsync();
                deletionSuccess = "True";
            }
            catch (Exception ex)
            {
                deletionSuccess = "False";
            }
        }

        public void ToasterStatus()
        {
            deletionSuccess = "";

        }

        public async Task getAuditHistory(string days)
        {
            try
            {
                var temp = Int32.Parse(days);
                //await _client.AuditAsync(temp);
                auditSuccess = "True";
            }
            catch
            {
                auditSuccess = "False";
            }
        }
    }
}
