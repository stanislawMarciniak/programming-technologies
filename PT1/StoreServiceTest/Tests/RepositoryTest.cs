using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StoreService.Data;
using StoreService.Logic;
using System.Collections.Generic;
using System.Text;

namespace StoreService.Tests
{
    [TestClass]
    public class RepositoryTests
    {
        private Repository repository;

        [TestInitialize]
        public void Initialize()
        {
            DataGenerator dataGenerator = new DataGenerator();
            DataContext dataContext = dataGenerator.GenerateData();
            repository = new Repository(dataContext);
        }

        [TestMethod]
        public void InitialRepositoryState()
        {
            Assert.IsTrue(repository.GetAllClients().Count.Equals(2));
            Assert.IsTrue(repository.GetAllEvents().Count.Equals(2));
            Assert.IsTrue(repository.GetAllStates().Count.Equals(4));
        }


        //Clients

        [TestMethod]
        public void AddClientTest()
        {
            Client client = new Client(103, "John", "Smith", "js@example.com");
            repository.AddClient(client);
            Assert.IsTrue(repository.GetClientByID(103).Equals(client));
        }

        [TestMethod]
        public void DeleteExsistingClientTest()
        {
            Client exsistingClient = repository.GetClientByID(101);
            repository.DeleteClient(exsistingClient);
            Assert.ThrowsException<KeyNotFoundException>(
                () => repository.GetClientByID(101));
        }

        [TestMethod]
        public void DeleteNonExsistingClientTest()
        {
            Client nonExsistingClient = new Client(333, "John", "Smith", "js@example.com");
            Assert.ThrowsException<KeyNotFoundException>(
                () => repository.DeleteClient(nonExsistingClient));
        }

        [TestMethod]
        public void NoSuchClientIDTest()
        {
            Assert.IsTrue(repository.NoSuchClientID(5));
            Assert.IsFalse(repository.NoSuchClientID(101)); 
        }

        [TestMethod]
        public void GetAllClientsIDsTest()
        {
            List<int> idList = repository.GetAllClients().Select(c => c.ClientID).ToList();
            CollectionAssert.AreEqual(repository.GetAllClientsIDs(), idList);
        }


        //Items

        [TestMethod]
        public void AddItemTest()
        {
            Item computer = new Item(1000, 5000, Category.electronics);
            repository.AddItem(computer);
            Assert.IsTrue(repository.GetItemByID(1000).Equals(computer));
        }

        [TestMethod]
        public void DeleteExsistingItemTest()
        {
            Item exsistingItem = repository.GetItemByID(1);
            repository.DeleteItem(exsistingItem);
            Assert.ThrowsException<KeyNotFoundException>(
                () => repository.DeleteItem(exsistingItem));
        }

        [TestMethod]
        public void DeleteNonExsistingItemTest()
        {
            Item nonExsistingItem = new Item(555, 50, Category.home);
            Assert.ThrowsException<KeyNotFoundException>(
                () => repository.DeleteItem(nonExsistingItem));
        }

        [TestMethod]
        public void NoSuchItemIDTest()
        {
            Assert.IsTrue(repository.NoSuchItemID(555));
            Assert.IsFalse(repository.NoSuchItemID(2));
        }

        [TestMethod]
        public void GetAllItemsTest()
        {
            List<int> idListFromItems = repository.GetAllItems().Select(i => i.ItemID).ToList();
            List<int> idList = repository.GetAllItemsIDs().ToList();
            CollectionAssert.AreEqual(idListFromItems, idList);
        }


        //State

        [TestMethod]
        public void DeleteBeforeAddedStateTest()
        {
            Item item = new Item(5, 90, Category.games);
            State state = new State(item, 4);
            Assert.ThrowsException<Exception>(
               () => repository.DeleteState(state));
            Assert.IsTrue(repository.NoSuchState(state));
        }

        [TestMethod]
        public void DeleteAfterAddedStateTest()
        {
            Item item = new Item(5, 90, Category.games);
            State state = new State(item, 4);
            repository.AddState(state);
            Assert.IsTrue(repository.GetAllStates().Contains(state));
            repository.DeleteState(state);
            Assert.IsFalse(repository.GetAllStates().Contains(state));
        }
        

        [TestMethod]
        public void RandomDataTest()
        {
            RandomDataGenerator random = new RandomDataGenerator();
            repository = new Repository(random.GenerateData());

            Assert.IsTrue(IsUnique(repository.GetAllClientsIDs()));
            Assert.IsTrue(IsUnique(repository.GetAllItemsIDs()));
        }

        private bool IsUnique(IEnumerable<int> list)
        {
            HashSet<int> uniqueValues = new HashSet<int>();

            foreach (int id in list)
            {
                if (!uniqueValues.Add(id))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
