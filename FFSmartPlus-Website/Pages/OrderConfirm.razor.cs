﻿using ClientAPI;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using System.IdentityModel.Tokens.Jwt;
using System.Diagnostics.CodeAnalysis;

namespace FFSmartPlus_Website.Pages
{
    public partial class OrderConfirm
    {
        [Inject]
        public FFSBackEnd? _client { get; set; }
        [Inject]
        protected IMatToaster Toaster { get; set; }

        public ICollection<SupplierOrderDto> orderItems = new List<SupplierOrderDto>();


        public string newQuantity;
        public string additionSuccess;
        public List<string> currentUserRole;

        protected async override Task OnInitializedAsync()
        {
            CurrentUserRoles? i = new CurrentUserRoles();
            currentUserRole = CurrentUserRoles._role;
            StateHasChanged();
            orderItems = await _client.GenerateOrderAsync();
        }


        public async Task changeQuantity(long id, string quantity)
        {
            ItemRequestDto changedQuantityRequest = new ItemRequestDto();
            OrderRequestDto changedQuantityOrder = new OrderRequestDto();
            changedQuantityRequest.Id = id;
            ICollection<ItemRequestDto> itemRequests = new List<ItemRequestDto>();
            try
            {
                changedQuantityRequest.Quantity = Convert.ToDouble(quantity);

                itemRequests.Add(changedQuantityRequest);
                changedQuantityOrder.Items = itemRequests;
                var cobIDResponse = await _client.ConfirmOrderByIDsAsync(changedQuantityOrder);
                if (cobIDResponse)
                {
                    additionSuccess = "True";
                    orderItems = await _client.GenerateOrderAsync();

                }
                else
                {
                    additionSuccess = "False";

                }
            }
            catch (Exception ex)
            {
                additionSuccess = "False";
                Console.WriteLine(ex);
            }
        }


        public async Task ToasterStatus()
        {
           additionSuccess = "";

        }

    }
}
