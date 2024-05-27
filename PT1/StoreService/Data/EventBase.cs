using System;
using System.Collections.Generic;
using System.Text;

namespace StoreService.Data
{
    public abstract class EventBase
    {
        private State state;
        private Client client;
        private DateTime purchaseDate;

        public State State => state;
        public Client Client => client;
        public DateTime PurchaseDate => purchaseDate;

        public EventBase(State _state, Client _client)
        {
            state = _state;
            client = _client;
            purchaseDate = DateTime.Now;
        }
    }
}