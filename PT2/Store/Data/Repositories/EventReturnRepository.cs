using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ReturnEventRepository
    {
        public List<EventReturn> GetAllReturnEvents()
        {
            using (var db = new StoreDataContext())
            {
                return db.EventReturns.Select(ev => ev).ToList();
            }
        }

        public EventReturn GetReturnEventById(int id)
        {
            using (var db = new StoreDataContext())
            {
                return db.ReturnEvents.FirstOrDefault(ev => ev.Id.Equals(id));
            }
        }

        public List<EventReturn> GetReturnEventsByClientId(int id)
        {
            using (var db = new StoreDataContext())
            {
                return db.EventReturns.Where(ev => ev.ClientId.Equals(id)).ToList();
            }
        }

        public List<EventReturn> GetReturnEventsByItemId(int id)
        {
            using (var db = new StoreDataContext())
            {
                return db.EventReturns.Where(ev => ev.ItemID.Equals(id)).ToList();
            }
        }

        public void AddReturnEvent(EventReturn e)
        {
            using (var db = new StoreDataContext())
            {
                db.EventReturns.InsertOnSubmit(e);
                db.SubmitChanges();
            }
        }

        public EventReturn GetMostRecentReturn()
        {
            using (var db = new StoreDataContext())
            {
                return db.EventReturns.Select(p => p).ToList().LastOrDefault();
            }
        }
    }
}
