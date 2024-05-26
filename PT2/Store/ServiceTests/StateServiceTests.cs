using Data.API;
using Data.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.API;
using ServiceTests.Mocks;
using System.Threading.Tasks;

namespace ServiceTests
{
    [TestClass]
    public class StateServiceTests
    {
        private readonly IDataRepository _repository = new MockDataRepository();

        [TestMethod]
        public async Task AddAndRetrieveStateTest()
        {
            IProductCRUD productCrud = IProductCRUD.CreateProductCRUD(_repository);
            await productCrud.AddProductAsync(1, "Product1", 50, 0);
            IProductDTO product = await productCrud.GetProductAsync(1);
            
            IStateCRUD stateCrud = IStateCRUD.CreateStateCRUD(_repository);
            await stateCrud.AddStateAsync(1, product.Id, 10);
            IStateDTO retrievedState = await stateCrud.GetStateAsync(1);

            Assert.IsNotNull(retrievedState);
            Assert.AreEqual(1, retrievedState.Id);
            Assert.AreEqual(1, retrievedState.productQuantity);
            Assert.AreEqual(10, retrievedState.productQuantity);
        }

        [TestMethod]
        public async Task UpdateStateTest()
        {
            IProductCRUD productCrud = IProductCRUD.CreateProductCRUD(_repository);
            await productCrud.AddProductAsync(2, "Product2", 50, 0);
            IProductDTO product = await productCrud.GetProductAsync(2);

            IStateCRUD stateCrud = IStateCRUD.CreateStateCRUD(_repository);
            await stateCrud.AddStateAsync(2, product.Id, 10);
            IStateDTO retrievedState = await stateCrud.GetStateAsync(2);

            await stateCrud.UpdateStateAsync(2, product.Id, 100);
            IStateDTO updatedState = await stateCrud.GetStateAsync(2);

            Assert.IsNotNull(updatedState);
            Assert.AreEqual(2, updatedState.Id);
            Assert.AreEqual(2, updatedState.productQuantity);
            Assert.AreEqual(100, updatedState.productQuantity);
        }

        [TestMethod]
        public async Task DeleteStateTest()
        {
            IProductCRUD productCrud = IProductCRUD.CreateProductCRUD(_repository);
            await productCrud.AddProductAsync(1, "Product1", 50, 0);
            IProductDTO product = await productCrud.GetProductAsync(1);

            IStateCRUD stateCrud = IStateCRUD.CreateStateCRUD(_repository);
            await stateCrud.AddStateAsync(1, product.Id, 10);
            IStateDTO retrievedState = await stateCrud.GetStateAsync(1);

            await stateCrud.DeleteStateAsync(1);
            var deletedState = await stateCrud.GetStateAsync(1);

            Assert.IsNull(deletedState);
        }
    }
}
