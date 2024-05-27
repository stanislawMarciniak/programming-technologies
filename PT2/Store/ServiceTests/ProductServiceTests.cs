using Data.API;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.API;
using ServiceTests.Mocks;

namespace ServiceTests
{
    [TestClass]
    public class ProductServiceTests
    {
        private readonly IDataRepository _repository = new MockDataRepository();

        [TestMethod]
        public async Task AddAndRetrieveProductTest()
        {
            IProductCRUD productCrud = IProductCRUD.CreateProductCRUD(_repository);
            await productCrud.AddProductAsync(1, "Product1", 100, 0);

            IProductDTO retrievedProduct = await productCrud.GetProductAsync(1);

            Assert.IsNotNull(retrievedProduct);
            Assert.AreEqual(1, retrievedProduct.Id);
            Assert.AreEqual("Product1", retrievedProduct.Name);
            Assert.AreEqual(100, retrievedProduct.Price);
            Assert.AreEqual(0, retrievedProduct.Pegi);
        }

        [TestMethod]
        public async Task UpdateProductTest()
        {
            IProductCRUD productCrud = IProductCRUD.CreateProductCRUD(_repository);
            await productCrud.AddProductAsync(2, "Product2", 50, 0);
            
            await productCrud.UpdateProductAsync(2, "Product2Updated", 70, 0);
            IProductDTO updatedProduct = await productCrud.GetProductAsync(2);

            Assert.IsNotNull(updatedProduct);
            Assert.AreEqual(2, updatedProduct.Id);
            Assert.AreEqual("Product2Updated", updatedProduct.Name);
            Assert.AreEqual(70, updatedProduct.Price);
            Assert.AreEqual(0, updatedProduct.Pegi);
        }

        [TestMethod]
        public async Task DeleteProductTest()
        {
            IProductCRUD productCrud = IProductCRUD.CreateProductCRUD(_repository);
            await productCrud.AddProductAsync(3, "Product3", 200, 0);

            IProductDTO testProduct = await productCrud.GetProductAsync(3);
            Assert.IsNotNull(testProduct);

            // Delete the product
            await productCrud.DeleteProductAsync(3);

            // Product should not exist - cannot be retrieved
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await productCrud.GetProductAsync(3));
        }
    }
}
