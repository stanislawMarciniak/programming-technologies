using System;
using System.Collections.Generic;
using System.Text;
using StoreService.Data;
using StoreService.Tests;

namespace StoreServiceTest.Tests
{
    [TestClass]
    public class DataGeneratorTests
    {
        [TestMethod]
        public void DataGeneratorProducesExpectedData()
        {
            DataGenerator dataGenerator = new DataGenerator();
            DataContext context = dataGenerator.GenerateData();

            Assert.AreEqual(context.clients.Count, 2);
            Assert.AreEqual(context.items.Count, 4);
            Assert.AreEqual(context.states.Count, 4);
            Assert.AreEqual(context.events.Count, 2);
        }

        [TestMethod]
        public void RandomDataGeneratorProducesUniqueData()
        {
            RandomDataGenerator randomDataGenerator = new RandomDataGenerator();
            DataContext context = randomDataGenerator.GenerateData();

            Assert.IsTrue(IsUnique(context.clients.Select(c => c.ClientID)));
            Assert.IsTrue(IsUnique(context.items.Keys));
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