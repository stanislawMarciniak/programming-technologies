using System;
using System.Collections.Generic;
using System.Text;
using StoreService.Data;

namespace StoreService.Tests
{
    class DataGenerator : IDataGenerator
    {
        public DataContext GenerateData()
        {
            DataContext context = new DataContext();

            // Clients
            Client client1 = new Client(101, "A", "B", "ab@example.com");
            Client client2 = new Client(102, "C", "D", "cd@example.com");

            context.clients.Add(client1);
            context.clients.Add(client2);

            // Items 
            Item item1 = new Item(1, 20, Category.electronics);
            Item item2 = new Item(2, 30, Category.home);
            Item item3 = new Item(3, 40, Category.garden);
            Item item4 = new Item(4, 50, Category.health);


            context.items.Add(1, item1);
            context.items.Add(2, item2);
            context.items.Add(3, item3);
            context.items.Add(4, item4);

            // States 
            State state1 = new State(context.items[1], 5);
            State state2 = new State(context.items[2], 2);
            State state3 = new State(context.items[3], 0); 
            State state4 = new State(context.items[4], 1);

            context.states.Add(state1);
            context.states.Add(state2);
            context.states.Add(state3);
            context.states.Add(state4);

            // Events
            EventPurchase eventPurchase1 = new EventPurchase(state1, client1, 1);
            EventPurchase eventPurchase2 = new EventPurchase(state2, client2, 1);

            context.events.Add(eventPurchase1);
            context.events.Add(eventPurchase2);

            return context;
        }
    }
}