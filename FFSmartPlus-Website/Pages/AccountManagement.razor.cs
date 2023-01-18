using ClientAPI;
using MatBlazor;
using Microsoft.AspNetCore.Components;

namespace FFSmartPlus_Website.Pages
{
    public partial class AccountManagement
    {
        [Inject]
        public FFSBackEnd? _client { get; set; }
        [Inject]
        protected IMatToaster Toaster { get; set; }

        public string inputUsername;
        public string inputRole;

        public ICollection<string> users;

        public async Task getUsers()
        {
            users = await _client.AllAsync();
        }

        public async Task addRole(string username, string role)
        {
            await _client.AddRoleAsync(username, role);
        }


        public async Task deleteRole(string username, string role)
        {
            await _client.RemoveRoleAsync(username, role);
        }

    }
}
