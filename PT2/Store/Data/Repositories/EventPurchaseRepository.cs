using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class PurchaseEventRepository
    {
        public List<PurchaseEvent> GetAllPurchaseEvents()
        {
            using (var db = new StoreDataContext())
            {
                return db.PurchaseEvents.Select(ev => ev).ToList();
            }
        }

        public PurchaseEvent GetPurchaseEventById(int id)
        {
            using (var db = new StoreDataContext())
            {
                return db.PurchaseEvents.FirstOrDefault(ev => ev.Id.Equals(id));
            }
        }

        public List<PurchaseEvent> GetPurchaseEventsByClientId(int id)
        {
            using (var db = new StoreDataContext())
            {
                return db.PurchaseEvents.Where(ev => ev.ClientId.Equals(id)).ToList();
            }
        }

        public List<PurchaseEvent> GetPurchaseEventsByProductId(int id)
        {
            using (var db = new StoreDataContext())
            {
                return db.PurchaseEvents.Where(ev => ev.ProductId.Equals(id)).ToList();
            }
        }

        public void AddPurchaseEvent(PurchaseEvent e)
        {
            using (var db = new StoreDataContext())
            {
                db.PurchaseEvents.InsertOnSubmit(e);
                db.SubmitChanges();
            }
        }

        public void DeletePurchaseEvent(int id)
        {
            using (var db = new StoreDataContext())
            {
                PurchaseEvent eventToDelete = db.PurchaseEvents.FirstOrDefault(e => e.Id.Equals(id));

                if (eventToDelete != null)
                {
                    db.PurchaseEvents.DeleteOnSubmit(eventToDelete);
                    db.SubmitChanges();
                }
            }
        }

        public PurchaseEvent GetMostRecentPurchase()
        {
            using (var db = new StoreDataContext())
            {
                return db.PurchaseEvents.Select(p => p).ToList().LastOrDefault();
            }
        }

        public PurchaseEvent GetMostRecentByClientIdAndProductId(int clientId, int productId)
        {
            using (var db = new StoreDataContext())
            {
                return db.PurchaseEvents.Where(p =>
                    (p.ClientId.Equals(clientId) && (p.ProductId.Equals(productId))))
                    .ToList().LastOrDefault();
            }
        }
    }
}
