using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data.API;
using Data.Database;

namespace DataTests
{
    [TestClass]
    [DeploymentItem("TestingDatabase.mdf")]
    public class ProductTests
    {
        private static string connectionString;
        private readonly IDataRepository _dataRepository = IDataRepository.CreateDatabase(IDataContext.CreateContext(connectionString));

        [ClassInitialize]
        public static void ClassInitializeMethod(TestContext context)
        {
            string dbRelativePath = @"TestingDatabase.mdf";
            string projectRootDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string dbPath = Path.Combine(projectRootDir, dbRelativePath);
            FileInfo databaseFile = new FileInfo(dbPath);
            Assert.IsTrue(databaseFile.Exists, $"{Environment.CurrentDirectory}");
            connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;Connect Timeout=30;";
        }

        [TestMethod]
        public async Task AddAndRetrieveProductTest()
        {
            int testProductId = 202;
            
            await _dataRepository.AddProductAsync(testProductId, "Movie example", 300, 18);
            IMovie testProduct = await _dataRepository.GetProductAsync(testProductId);

            Assert.IsNotNull(testProduct);
            Assert.AreEqual(testProductId, testProduct.Id);
            Assert.AreEqual("Movie example", testProduct.MovieName);
            Assert.AreEqual(300, testProduct.Price);
            Assert.AreEqual(18, testProduct.AgeRestriction);

            Assert.IsNotNull(await _dataRepository.GetAllProductsAsync());
            Assert.IsTrue(await _dataRepository.GetProductsCountAsync() > 0);

            await _dataRepository.DeleteProductAsync(testProductId);
        }

        [TestMethod]
        public async Task UpdateAndDeleteProductTest()
        {
            int testProductId = 1;

            await _dataRepository.AddProductAsync(testProductId, "Movie example", 300, 18);
            await _dataRepository.UpdateProductAsync(testProductId, "Movie example - updated", 350, 16);
            
            IMovie updatedProduct = await _dataRepository.GetProductAsync(testProductId);

            Assert.IsNotNull(updatedProduct);
            Assert.AreEqual(testProductId, updatedProduct.Id);
            Assert.AreEqual("Movie example - updated", updatedProduct.MovieName);
            Assert.AreEqual(350, updatedProduct.Price);
            Assert.AreEqual(16, updatedProduct.AgeRestriction);

            await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.UpdateProductAsync(999, "Movie example - updated", 350, 16));
            await _dataRepository.DeleteProductAsync(testProductId);

            await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.GetProductAsync(testProductId));
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.DeleteProductAsync(testProductId));
        }
    }
}
