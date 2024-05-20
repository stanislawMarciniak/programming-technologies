using System.Collections.Generic;

namespace Service
{
    public class ItemService
    {
        private ItemRepository repository = new ItemRepository();
        private PurchaseEventRepository purchaseRepository = new PurchaseEventRepository();
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
            if (Item == null || ContainsItemWithName(Item._ItemName))
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

        public List<ItemModel> GetItemsByCategory(ItemCategory category)
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
            if (model == null || !ItemExists(model._id))
            {
                return false;
            }

            repository.UpdateItem(MapModelDetails(model));
            return true;
        }

        public List<ItemCategory> GetAllCategories()
        {
            return repository.GetAllCategories();
        }

        public ItemCategory GetItemCategoryByName(string category)
        {
            return repository.GetCategoryByName(category);
        }

        public bool HasNoPurchases(int id)
        {
            return purchaseRepository.GetPurchaseEventsByItemId(id).Count.Equals(0);
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
                Id = model._id,
                ItemName = model._ItemName,
                Price = model._price,
                Category = model._category
            };
        }

        private ItemModel MapItemDetails(Item Item)
        {
            return new ItemModel()
            {
                _id = Item.Id,
                _ItemName = Item.ItemName,
                _price = Item.Price,
                _category = Item.Category
            };
        }
    }
}
