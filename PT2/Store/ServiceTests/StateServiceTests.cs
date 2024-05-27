using Data.API;
using Data.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.API;
using ServiceTests.Mocks;
using System.Collections.Specialized;
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
            await productCrud.AddProductAsync(1, "Product1", 50, 6);
            IProductDTO product = await productCrud.GetProductAsync(1);

            IStateCRUD stateCrud = IStateCRUD.CreateStateCRUD(_repository);
            await stateCrud.AddStateAsync(1, product.Id, 10);
            IStateDTO testedState = await stateCrud.GetStateAsync(1);

            Assert.IsNotNull(testedState);
            Assert.AreEqual(1, testedState.Id);
            Assert.AreEqual(1, testedState.productId);
            Assert.AreEqual(10, testedState.productQuantity);
        }

        [TestMethod]
        public async Task UpdateStateTest()
        {
            IProductCRUD productCrud = IProductCRUD.CreateProductCRUD(_repository);
            await productCrud.AddProductAsync(2, "Product2", 50, 0);
            IProductDTO product = await productCrud.GetProductAsync(2);

            IStateCRUD stateCrud = IStateCRUD.CreateStateCRUD(_repository);
            await stateCrud.AddStateAsync(2, product.Id, 10);

            await stateCrud.UpdateStateAsync(2, product.Id, 100);
            IStateDTO updatedState = await stateCrud.GetStateAsync(2);

            Assert.IsNotNull(updatedState);
            Assert.AreEqual(2, updatedState.Id);
            Assert.AreEqual(2, updatedState.productId);
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

            // Delete the state
            await stateCrud.DeleteStateAsync(1);

            // State should not exist - cannot be retrieved
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await stateCrud.GetStateAsync(1));
        }
    }
}
