using System;
using System.Collections.Generic;

namespace StoreService.Data
{
    public class DataContext
    {
        public List<Client> clients = new List<Client>();
        public Dictionary<int, Item> items= new Dictionary<int, Item>();
        public List<State> states = new List<State>();
        public List<EventBase> events = new List<EventBase>();
    }
}