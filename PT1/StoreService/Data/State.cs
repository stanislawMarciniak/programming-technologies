using System;
using System.Collections.Generic;
using System.Text;

namespace StoreService.Data
{
    public class State
    {
        private Item item;
        private bool isAvailable;
        private int quantity;

        public Item Item => item;
        public bool IsAvailable => isAvailable;
        public int Quantity
        {
            get => quantity;
            private set => quantity = value;  
        }

        public State(Item _item, int _quantity)
        {
            item = _item;
            quantity = _quantity;
            isAvailable = _quantity > 0;    // Initial check for availability
        }
    }
}
