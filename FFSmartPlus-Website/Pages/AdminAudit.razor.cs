using ClientAPI;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text;
using System;


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
        public string streamString;


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
                var daysInt = Int32.Parse(days);
                ICollection < AuditDto > auditList = new List<AuditDto>();
                auditList = await _client.AuditAllAsync(daysInt);
                await DownloadText(auditList.ToList());
                await DownloadFileFromStream();
                StateHasChanged();
                auditSuccess = "True";
            }
            catch
            {
                auditSuccess = "False";
            }
        }

        //function below has been derived: https://codereview.stackexchange.com/questions/249241/creating-a-csv-stream-from-an-object
        protected async Task DownloadText(List<AuditDto> auditDto)
        {
            List<string> headers = typeof(AuditDto).GetProperties().Select(x => x.Name).ToList();
            streamString = string.Join(",", headers) + Environment.NewLine;

            foreach (AuditDto audit in auditDto)
            {
                List<object> values = typeof(AuditDto).GetProperties().Select(prop => prop.GetValue(audit)).ToList();
                string ss = string.Join(",", values);
                streamString += $"{ss}{Environment.NewLine}";
            }

        }

        //functions below have been derived from https://learn.microsoft.com/en-us/aspnet/core/blazor/file-downloads?view=aspnetcore-7.0
        private Stream GetFileStream(string stream)
        {

            byte[] bytes = Encoding.ASCII.GetBytes(stream);

            
            var fileStream = new MemoryStream(bytes);

            return fileStream;
        }

        private async Task DownloadFileFromStream()
        {
            var fileStream = GetFileStream(streamString);
            var fileName = "Audit.csv";

            using var streamRef = new DotNetStreamReference(stream: fileStream);

            await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }
    }
}

