using System;
using System.Collections.Generic;
using System.Text;

namespace StoreService.Data
{
    public class EventPurchase : EventBase
    {
        private int quantity;
        private decimal totalPrice;

        public int Quantity => quantity;
        public decimal TotalPrice => totalPrice;

        public EventPurchase(State state, Client client, int _quantity, decimal _totalPrice) : base(state, client)
        {
            quantity = _quantity;
            totalPrice = _totalPrice;
        }
    }
}
