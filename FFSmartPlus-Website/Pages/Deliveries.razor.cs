﻿using ClientAPI;
using MatBlazor;
using Microsoft.AspNetCore.Components;

namespace FFSmartPlus_Website.Pages
{
    public partial class Deliveries
    {
        

        [Inject]
        public FFSBackEnd? _client { get; set; }
        [Inject]
        protected IMatToaster Toaster { get; set; }

        public CurrentUserRoles currentUser = new CurrentUserRoles();
        public DateTime inputOrderDate;
        public string inputOrderId;
        public ActiveOrdersDto deliveriesOnDate;
        public OrderConfirmationDTO orderConfirmation = new OrderConfirmationDTO();
        public async Task getDeliveriesOnDate(string id, DateTime date)
        {
            long orderId = long.Parse(id);
            deliveriesOnDate = await _client.DeliveryAsync(orderId, date);
            //calls the get method to retrieve a list orders
        }

        public async Task confirmOrder(string id, string quantity, DateTime date )
        {
            //this will change
            NewUnitDto newUnit = new NewUnitDto();
            newUnit.Quantity = Int32.Parse(quantity);
            newUnit.ExpiryDate = date;
            orderConfirmation.OrderLogId = long.Parse(id);
            orderConfirmation.UnitDeliver = newUnit;
            await _client.ConfirmAsync(orderConfirmation);
        }
    }
}