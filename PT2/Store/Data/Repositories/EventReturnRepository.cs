using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ReturnEventRepository
    {
        public List<ReturnEvent> GetAllReturnEvents()
        {
            using (var db = new StoreDataContext())
            {
                return db.ReturnEvents.Select(ev => ev).ToList();
            }
        }

        public ReturnEvent GetReturnEventById(int id)
        {
            using (var db = new StoreDataContext())
            {
                return db.ReturnEvents.FirstOrDefault(ev => ev.Id.Equals(id));
            }
        }

        public List<ReturnEvent> GetReturnEventsByClientId(int id)
        {
            using (var db = new StoreDataContext())
            {
                return db.ReturnEvents.Where(ev => ev.ClientId.Equals(id)).ToList();
            }
        }

        public List<ReturnEvent> GetReturnEventsByProductId(int id)
        {
            using (var db = new StoreDataContext())
            {
                return db.ReturnEvents.Where(ev => ev.ProductId.Equals(id)).ToList();
            }
        }

        public void AddReturnEvent(ReturnEvent e)
        {
            using (var db = new StoreDataContext())
            {
                db.ReturnEvents.InsertOnSubmit(e);
                db.SubmitChanges();
            }
        }

        public ReturnEvent GetMostRecentReturn()
        {
            using (var db = new StoreDataContext())
            {
                return db.ReturnEvents.Select(p => p).ToList().LastOrDefault();
            }
        }
    }
}
