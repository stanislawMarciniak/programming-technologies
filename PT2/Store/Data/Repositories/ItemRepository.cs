using System.Collections.Generic;
using System.Linq;

namespace Data
{
    public class ItemRepository
    {
        public List<Item> GetAllItems()
        {
            using (var db = new StoreDataContext())
            {
                return db.Items.Select(Item => Item).ToList();
            }
        }

        public Item GetItemById(int id)
        {
            using (var db = new StoreDataContext())
            {
                return db.Items.FirstOrDefault(Item => Item.Id.Equals(id));
            }
        }
      
        public List<Item> GetItemsByCategory(String category)
        {
            using (var db = new StoreDataContext())
            {
                return db.Items.Where(Item => Item.Category.Equals(category)).ToList();
            }
        }

        public List<Item> GetItemsByPrice(float price)
        {
            using (var db = new StoreDataContext())
            {
                return db.Items.Where(Item => Item.Price.Equals(price)).ToList();
            }
        }

        public Item GetItemByName(string name)
        {
            using (var db = new StoreDataContext())
            {
                return db.Items.FirstOrDefault(Item => Item.ItemName.Equals(name));
            }
        }

        public List<Item> GetItemsCheaperThan(float price)
        {
            using (var db = new StoreDataContext())
            {
                return db.Items.Where(Item => Item.Price < price).ToList();
            }
        }

        public List<Item> GetItemsMoreExpensiveThan(float price)
        {
            using (var db = new StoreDataContext())
            {
                return db.Items.Where(Item => Item.Price > price).ToList();
            }
        }

        public Item GetLastItem()
        {
            using (var db = new StoreDataContext())
            {
                return db.Items.Select(Item => Item).ToList().LastOrDefault();
            }
        }

        public void AddItem(Item Item)
        {
            using (var db = new StoreDataContext())
            {
                db.Items.InsertOnSubmit(Item);
                db.SubmitChanges();
            }
        }

        public void DeleteItem(int id)
        {
            using (var db = new StoreDataContext())
            {
                Item ItemToDelete = db.Items.FirstOrDefault(Item => Item.Id.Equals(id));

                if (ItemToDelete != null)
                {
                    db.Items.DeleteOnSubmit(ItemToDelete);
                    db.SubmitChanges();
                }
            }
        }

        public void UpdateItem(Item p)
        {
            using (var db = new StoreDataContext())
            {
                Item ItemToUpdate = db.Items.FirstOrDefault(Item => Item.Id.Equals(p.ItemID));

                if (ItemToUpdate != null)
                {
                    ItemToUpdate.ItemName = p.ItemName;
                    ItemToUpdate.Category = p.Category;
                    ItemToUpdate.Price = p.Price;
                    db.SubmitChanges();
                }
            }
        }

        public List<String> GetAllCategories()
        {
            using (var db = new StoreDataContext())
            {
                return db.ItemCategories.Select(category => category).ToList();
            }
        }

        public String GetCategoryByName(string category)
        {
            using (var db = new StoreDataContext())
            {
                return db.ItemCategories.FirstOrDefault(c => c.Category.Equals(category));
            }
        }
    }
}
