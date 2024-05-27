using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace StoreService.Data
{
    public class Item
    {
        private int itemID;
        private double price;
        private Category category;

        public int ItemID => itemID;
        public double Price => price;
        public Category Category => category;

        public Item(int _itemID, double _price, Category _category)
        {
            itemID = _itemID;
            price = _price;
            category = _category;
        }
    }

    public enum Category
    {
        electronics,
        home,
        garden,
        health,
        books,
        games,
        food
    }
}