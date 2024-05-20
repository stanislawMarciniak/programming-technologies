using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoreService.Logic;
using StoreService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using StoreServiceTest.Tests;

namespace StoreService.Tests
{
    [TestClass]
    public class DataServiceTest
    {
        private DataService service;
        private Mock<IRepository> mockRepository;
        private DataContext context;

        [TestInitialize]
        public void Initialize()
        {
            /*            DataGenerator contentGenerator = new DataGenerator();
                        service = new DataService(new Repository(contentGenerator.GenerateData()));*/
            var dataGenerator = new DataGenerator();
            context = dataGenerator.GenerateData();
            mockRepository = new Mock<IRepository>();
            SetupMockRepository();

            service = new DataService(mockRepository.Object);

        }

        private void SetupMockRepository()
        {
            // Clients
            mockRepository.Setup(repo => repo.GetAllClients()).Returns(context.clients);
            mockRepository.Setup(repo => repo.GetClientByID(It.IsAny<int>())).Returns<int>(id => context.clients.FirstOrDefault(c => c.ClientID == id));
            mockRepository.Setup(repo => repo.AddClient(It.IsAny<Client>())).Callback<Client>(client => context.clients.Add(client));
            mockRepository.Setup(repo => repo.DeleteClient(It.IsAny<Client>())).Callback<Client>(client => context.clients.Remove(client));

            // Items
            mockRepository.Setup(repo => repo.GetAllItems()).Returns(context.items.Values.ToList());
            mockRepository.Setup(repo => repo.GetItemByID(It.IsAny<int>())).Returns<int>(id => context.items.ContainsKey(id) ? context.items[id] : throw new KeyNotFoundException());
            mockRepository.Setup(repo => repo.AddItem(It.IsAny<Item>())).Callback<Item>(item => context.items.Add(item.ItemID, item));
            mockRepository.Setup(repo => repo.DeleteItem(It.IsAny<Item>())).Callback<Item>(item => context.items.Remove(item.ItemID));

            // States
            mockRepository.Setup(repo => repo.GetAllStates()).Returns(context.states);
            mockRepository.Setup(repo => repo.AddState(It.IsAny<State>())).Callback<State>(state => context.states.Add(state));
            mockRepository.Setup(repo => repo.DeleteState(It.IsAny<State>())).Callback<State>(state => context.states.Remove(state));

            // Events
            mockRepository.Setup(repo => repo.GetAllEvents()).Returns(context.events);
            mockRepository.Setup(repo => repo.AddEvent(It.IsAny<EventBase>())).Callback<EventBase>(e => context.events.Add(e));
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
            Assert.AreEqual(service.GetAllClients().Count, 1);
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
            Assert.AreEqual(4, initialItemIDList.Count); 
            service.AddItem(6, 40, Category.games);
            List<int> finalItemIDList = service.GetAllItems().Select(p => p.ItemID).ToList();
            Assert.AreEqual(4, finalItemIDList.Count);
        }

        [TestMethod]
        public void DeleteExistingItemTest()
        {
            Item i = service.GetItemByID(2);
            service.DeleteItem(i);
            Assert.ThrowsException<KeyNotFoundException>(
                () => service.GetItemByID(2));
        }


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
            Client c = service.GetClient(101);
            List<EventBase> itemEvents = service.GetAllClientEvents(101);
            Assert.IsTrue(itemEvents[0].Client.Equals(c));
        }

        [TestMethod]
        public void PurchaseActionTest()
        {
            service.PurchaseItem(4, 101, 1);
            Assert.ThrowsException<KeyNotFoundException>(
                () => service.GetItemByID(4));
            List<EventBase> itemEvents = service.GetAllClientEvents(101);
            Assert.IsTrue(itemEvents[1].Client.ClientID.Equals(101));
            Assert.IsTrue(itemEvents[1].State.Item.ItemID.Equals(4));
        }

        [TestMethod]
        public void ReturnActionTest()
        {
            Item item = service.GetItemByID(1);
            Client client = service.GetClient(101);
            service.PurchaseItem(1, 101, 1);
            service.ReturnItem(item, client, 1, 1, "abc");
            List<EventBase> itemEvents = service.GetAllClientEvents(101);
            Assert.IsTrue(itemEvents[2].Client.ClientID.Equals(101));
            Assert.IsTrue(itemEvents[2].State.Item.ItemID.Equals(1));
        }

    }
}