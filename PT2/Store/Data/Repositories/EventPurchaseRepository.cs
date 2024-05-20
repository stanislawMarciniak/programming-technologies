using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class EventPurchaseRepository
    {
        public EventPurchase GetEventsPurchaseById(int id)
        {
            using (var db = new ShopDataContext())
            {
                return db.EventPurchases.FirstOrDefault(ev => ev.Id.Equals(id));
            }
        }

        public List<EventPurchase> GetEventsPurchaseByClientId(int id)
        {
            using (var db = new ShopDataContext())
            {
                return db.EventPurchases.Where(ev => ev.ClientID.Equals(id)).ToList();
            }
        }

        public List<EventPurchase> GetEventsPurchaseByItemId(int id)
        {
            using (var db = new ShopDataContext())
            {
                return db.EventPurchases.Where(ev => ev.ItemID.Equals(id)).ToList();
            }
        }

        public List<EventPurchase> GetAllPurchaseEvents()
        {
            using (var db = new ShopDataContext())
            {
                return db.EventPurchases.Select(ev => ev).ToList();
            }
        }

        public void AddEventPurchase(EventPurchase e)
        {
            using (var db = new StoreDataContext())
            {
                db.EventPurchases.InsertOnSubmit(e);
                db.SubmitChanges();
            }
        }

        public void DeletePurchaseEvent(int id)
        {
            using (var db = new StoreDataContext())
            {
                EventPurchase eventToDelete = db.PurchaseEvents.FirstOrDefault(e => e.Id.Equals(id));

                if (eventToDelete != null)
                {
                    db.EventPurchases.DeleteOnSubmit(eventToDelete);
                    db.SubmitChanges();
                }
            }
        }

        public EventPurchase GetMostRecentPurchase()
        {
            using (var db = new StoreDataContext())
            {
                return db.EventPurchases.Select(p => p).ToList().LastOrDefault();
            }
        }

        public EventPurchase GetMostRecentByClientIdAndItemId(int clientId, int ItemId)
        {
            using (var db = new StoreDataContext())
            {
                return db.EventPurchases.Where(p =>
                    (p.Id.Equals(clientId) && (p.ItemId.Equals(ItemId))))
                    .ToList().LastOrDefault();
            }
        }

    }
}
