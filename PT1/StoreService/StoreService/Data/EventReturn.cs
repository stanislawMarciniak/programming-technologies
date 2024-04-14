using System;
using System.Collections.Generic;
using System.Text;

namespace StoreService.Data
{
    public class EventReturn : EventBase
    {
        private int quantityReturned;
        private decimal refundAmount;
        private String returnReason;

        public int QuantityReturned => quantityReturned;
        public decimal RefundAmount => refundAmount;
        public String ReturnReason => returnReason;  

        public EventReturn(State state, Client client, int _quantityReturned, decimal _refundAmount, String _returnReason)
            : base(state, client)
        {
            quantityReturned = _quantityReturned;
            refundAmount = _refundAmount;
            returnReason = _returnReason;
        }
    }
}
