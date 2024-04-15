using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoreService.Logic;
using StoreService.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreService.Tests
{
    [TestClass]
    public class DataServiceTest
    {
        DataService service;

        [TestInitialize]
        public void Initialize()
        {
            DataGenerator contentGenerator = new DataGenerator();
            service = new DataService(new Repository(contentGenerator.GenerateData()));
        }


        //Clients

        [TestMethod]
        public void AddClientTest()
        {
            service.AddClient(103, "John", "Smith", "js@example.com");
            Assert.AreEqual(service.GetAllClients().Count, 3);
        }

        [TestMethod]
        public void AddClientNotUniqueIdTest()
        {
            Assert.ThrowsException<Exception>(
                () => service.AddClient(101, "John", "Smith", "js@example.com"));
        }

        [TestMethod]
        public void DeleteExsistingClientTest()
        {
            Client exsistingClient = service.GetClient(101);
            service.DeleteClient(exsistingClient);

            Assert.AreEqual(service.GetAllClients().Count, 101);
        }

        [TestMethod]
        public void DeleteNonExsistingClientTest()
        {
            Client nonExsistingClient = new Client(333, "John", "Smith", "js@example.com");
            Assert.ThrowsException<KeyNotFoundException>(() =>
               service.DeleteClient(nonExsistingClient));

            Assert.AreEqual(service.GetAllClients().Count, 2);
        }


        //Items

        [TestMethod]
        public void AddItemTest()
        {
            int id = 1000;
            int price = 5000;
            Category cat = Category.games;
            service.AddItem(id, price, cat);
            Item boardGame = service.GetItemByID(1000);
            Assert.IsTrue(boardGame.ItemID == id && boardGame.Price == price && boardGame.Category == cat);
        }

        [TestMethod]
        public void GetAllItemsTest()
        {
            List<int> initialItemIDList = service.GetAllItems().Select(p => p.ItemID).ToList();
            Assert.IsTrue(initialItemIDList.Count.Equals(4));
            service.AddItem(6, 40, Category.games);
            List<int> finalItemIDList = service.GetAllItems().Select(p => p.ItemID).ToList();
            Assert.IsTrue(finalItemIDList.Count.Equals(5));
        }

/*        [TestMethod]
        public void DeleteExistingItemTest()
        {
            service.DeleteItem(item1);
            Assert.ThrowsException<KeyNotFoundException>(
                () => service.GetItemByID(2));

        }
        [TestMethod]
        public void DeleteNonExistingItemTest()
        {
            Assert.ThrowsException<KeyNotFoundException>(
                () => service.DeleteItem(10));
        }

*/
        // Events 

        [TestMethod]
        public void GetItemEventsTest()
        {
            Item i = service.GetItemByID(1);
            List<EventBase> itemEvents = service.GetAllItemEvents(i);
            Assert.IsTrue(itemEvents[0].State.Item.Equals(i));
        }

        [TestMethod]
        public void GetClientEventsTest()
        {
            Client c = service.GetClient(1);
            List<EventBase> itemEvents = service.GetAllClientEvents(1);
            Assert.IsTrue(itemEvents[0].Client.Equals(c));
        }

        [TestMethod]
        public void PurchaseActionTest()
        {
            service.PurchaseItem(4, 2, 1);
            Assert.ThrowsException<KeyNotFoundException>(
                () => service.GetItemByID(3));
            List<EventBase> itemEvents = service.GetAllClientEvents(2);
            Assert.IsTrue(itemEvents[1].Client.ClientID.Equals(2));
            Assert.IsTrue(itemEvents[1].State.Item.ItemID.Equals(4));
        }
/*
        [TestMethod]
        public void ReturnActionTest()
        {
            Item item = service.GetItemByID(1);
            service.PurchaseItem(1, 1, 1);
            service.ReturnItem(item, client, 1, 1);
            List<EventBase> itemEvents = service.GetAllClientEvents(1);
            Assert.IsTrue(itemEvents[2].Client.ClientID.Equals(1));
            Assert.IsTrue(itemEvents[2].State.Item.ItemID.Equals(1));
        }*/

    }
}