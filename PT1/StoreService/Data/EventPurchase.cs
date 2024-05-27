using System;
using System.Collections.Generic;
using System.Text;

namespace StoreService.Data
{
    public class EventPurchase : EventBase
    {
        private int quantity;
        private readonly double totalPrice;

        public int Quantity => quantity;
        public decimal TotalPrice => (decimal)(quantity * State.Item.Price);

        public EventPurchase(State state, Client client, int _quantity) : base(state, client)
        {
            quantity = _quantity;
        }
    }
}
