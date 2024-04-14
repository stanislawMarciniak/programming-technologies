using System;
using System.Collections.Generic;
using System.Text;

namespace StoreService.Data
{
    public class EventReturn : EventBase
    {
        private int quantityReturned;
        private readonly decimal refundAmount;
        private String returnReason;

        public int QuantityReturned => quantityReturned;
        public decimal RefundAmount => (decimal)(quantityReturned * State.Item.Price);

        public String ReturnReason => returnReason;  

        public EventReturn(State state, Client client, int _quantityReturned, String _returnReason)
            : base(state, client)
        {
            quantityReturned = _quantityReturned;
            returnReason = _returnReason;
        }
    }
}
