using Microsoft.VisualStudio.TestTools.UnitTesting;
using Presentation.Model.API;
using Presentation.ViewModel;
using Service.API;

namespace PresentationTests
{
    [TestClass]
    public class ProductViewModelTests
    {
        private readonly IErrorInformer _informer = new TextErrorInformer();

        [TestMethod]
        public void ProductMasterViewModelTests()
        {
            MockDataRepository mockDataRepository = new MockDataRepository();
            IProductCRUD mockProductCrud = new MockProductCRUD();

            IProductModelOperation operation = IProductModelOperation.CreateModelOperation(mockProductCrud);
            IProductMasterViewModel master = IProductMasterViewModel.CreateViewModel(operation, _informer);

            master.Name = "TestProduct";
            master.Price = 150;
            master.Pegi = 16;

            Assert.IsNotNull(master.CreateProduct);
            Assert.IsNotNull(master.RemoveProduct);

            Assert.IsTrue(master.CreateProduct.CanExecute(null));

            master.Price = -1;
            Assert.IsFalse(master.CreateProduct.CanExecute(null));

            master.Price = 150;
            Assert.IsTrue(master.RemoveProduct.CanExecute(null));
        }

        [TestMethod]
        public void ProductDetailViewModelTests()
        {
            MockDataRepository mockDataRepository = new MockDataRepository();
            IProductCRUD mockProductCrud = new MockProductCRUD();

            IProductModelOperation operation = IProductModelOperation.CreateModelOperation(mockProductCrud);
            IProductDetailViewModel detail = IProductDetailViewModel.CreateViewModel(1, "TestProduct", 150, 16,
                operation, _informer);

            Assert.AreEqual(1, detail.Id);
            Assert.AreEqual("TestProduct", detail.Name);
            Assert.AreEqual(150, detail.Price);
            Assert.AreEqual(16, detail.Pegi);

            Assert.IsTrue(detail.UpdateProduct.CanExecute(null));
        }
    }
}
