using System.Collections.Generic;
using Data;
using Service.Models;

namespace Service.Services
{
    public class ItemService
    {
        private ItemRepository repository = new ItemRepository();
        private EventPurchaseRepository purchaseRepository = new EventPurchaseRepository();
        private ReturnEventRepository returnRepository = new ReturnEventRepository();

        public List<ItemModel> GetAllItems()
        {
            List<ItemModel> models = new List<ItemModel>();

            foreach (var Item in repository.GetAllItems())
            {
                models.Add(MapItemDetails(Item));
            }
            return models;
        }

        public bool AddItem(ItemModel Item)
        {
            if (Item == null)
            {
                return false;
            }

            repository.AddItem(MapModelDetails(Item));
            return true;
        }

        public ItemModel GetItemById(int id)
        {
            Item Item = repository.GetItemById(id);

            return (Item is null) ? null : MapItemDetails(Item);
        }

        public List<ItemModel> GetItemsByCategory(String category)
        {
            List<ItemModel> models = new List<ItemModel>();

            foreach (var Item in repository.GetItemsByCategory(category))
            {
                models.Add(MapItemDetails(Item));
            }
            return models;
        }

        public List<ItemModel> GetItemsByPrice(float price)
        {
            List<ItemModel> models = new List<ItemModel>();

            foreach (var Item in repository.GetItemsByPrice(price))
            {
                models.Add(MapItemDetails(Item));
            }
            return models;
        }

        public ItemModel GetItemByName(string name)
        {
            return MapItemDetails(repository.GetItemByName(name));
        }

        public ItemModel GetLastlyAddedItem()
        {
            return MapItemDetails(repository.GetLastItem());
        }

        public List<ItemModel> GetItemsCheaperThan(float price)
        {
            List<ItemModel> models = new List<ItemModel>();

            foreach (var Item in repository.GetItemsCheaperThan(price))
            {
                models.Add(MapItemDetails(Item));
            }
            return models;

        }

        public List<ItemModel> GetItemsMoreExpensiveThan(float price)
        {
            List<ItemModel> models = new List<ItemModel>();

            foreach (var Item in repository.GetItemsMoreExpensiveThan(price))
            {
                models.Add(MapItemDetails(Item));
            }
            return models;

        }

        public bool DeleteItem(int id)
        {
            if (CanBeDeleted(id))
            {
                repository.DeleteItem(id);
                return true;
            }

            return false;
        }

        public bool UpdateSelectedItem(ItemModel model)
        {
            if (model == null || !ItemExists(model._itemID))
            {
                return false;
            }

            repository.UpdateItem(MapModelDetails(model));
            return true;
        }

        public List<String> GetAllCategories()
        {
            return repository.GetAllCategories();
        }

        public String GetItemCategoryByName(String category)
        {
            return repository.GetCategoryByName(category);
        }

        public bool HasNoPurchases(int id)
        {
            return purchaseRepository.GetEventsPurchaseByItemId(id).Count.Equals(0);
        }

        public bool HasNoReturns(int id)
        {
            return returnRepository.GetReturnEventsByItemId(id).Count.Equals(0);
        }

        public bool CanBeDeleted(int id)
        {
            return HasNoPurchases(id) && HasNoReturns(id) && ItemExists(id);
        }

        public bool ContainsItemWithName(string name)
        {
            return repository.GetItemByName(name) != null;
        }

        public bool ItemExists(int id)
        {
            return repository.GetItemById(id) != null;
        }

        private Item MapModelDetails(ItemModel model)
        {
            return new Item()
            {
                ItemID = model._itemID,
                Price = (decimal)model._price,
                Category = model._itemCategory
            };
        }

        private ItemModel MapItemDetails(Item Item)
        {
            return new ItemModel()
            {
                _itemID = Item.ItemID,
                _price = (double)Item.Price,
                _itemCategory = Item.Category
            };
        }
    }
}
