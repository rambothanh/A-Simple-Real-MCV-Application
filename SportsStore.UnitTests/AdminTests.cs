using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Products()
        {
            //Arrange
            //Tạo mock repo
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "p1"},
                new Product {ProductID = 2, Name = "p2"},
                new Product {ProductID = 3, Name = "p3"}
            });

            //Tạo controller
            AdminController adminController = new AdminController(mock.Object);

            //Action
            Product[] result = ((IEnumerable<Product>)adminController.Index().ViewData.Model)
                .ToArray();

            //Assert
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual(result[0].Name, "p1");
            Assert.AreEqual(result[1].Name, "p2");
            Assert.AreEqual(result[2].Name, "p3");

        }

        [TestMethod]
        public void Can_Edit_Product()
        {
            //Arrange
            //Tạo mock repo
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "p1"},
                new Product {ProductID = 2, Name = "p2"},
                new Product {ProductID = 3, Name = "p3"}
            });

            //Tạo controller
            AdminController adminController = new AdminController(mock.Object);

            //Action
            Product p1 = adminController.Edit(1).ViewData.Model as Product;
            Product p2 = adminController.Edit(2).ViewData.Model as Product;
            Product p3 = adminController.Edit(3).ViewData.Model as Product;


            //Assert

            Assert.AreEqual(p1.Name, "p1");
            Assert.AreEqual(p2.Name, "p2");
            Assert.AreEqual(p3.Name, "p3");
            Assert.AreEqual(p1.ProductID, 1);
            Assert.AreEqual(p2.ProductID, 2);
            Assert.AreEqual(p3.ProductID, 3);

        }

        [TestMethod]
        public void Can_None_Existent_Product()
        {
            //Arrange
            //Tạo mock repo
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "p1"},
                new Product {ProductID = 2, Name = "p2"},
                new Product {ProductID = 3, Name = "p3"}
            });

            //Tạo controller
            AdminController adminController = new AdminController(mock.Object);

            //Action

            Product p4 = adminController.Edit(4).ViewData.Model as Product;


            //Assert
            Assert.IsNull(p4);

        }

        [TestMethod]
        public void Can_Save_Valid_Change()
        {
            //Arrange
            //Tạo mock repo
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            //Tạo controller
            AdminController adminController = new AdminController(mock.Object);

            //Tạo product
            Product product = new Product { Name = "test" };

            //Action
            //Save product
            ActionResult result = adminController.Edit(product);


            //Assert
            //check that the repository was called
            mock.Verify(m => m.SaveProduct(product));
            //check the method result type
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));

        }

        [TestMethod]
        public void Cannot_Save_InValid_Change()
        {
            //Arrange
            //Tạo mock repo
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            //Tạo controller
            AdminController adminController = new AdminController(mock.Object);

            //Tạo product
            Product product = new Product { Name = "test" };

            //add an error to the model state
            adminController.ModelState.AddModelError("error", "error");

            //Action
            //Save product
            ActionResult result = adminController.Edit(product);


            //Assert
            //check that the repository was not called
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());
            //check the method result type
            Assert.IsInstanceOfType(result, typeof(ViewResult));

        }

        [TestMethod]
        public void Can_Delete_Valid_Product()
        {
            //Arrange
            //Tạo một product sẽ được delete
            Product delProduct = new Product{ ProductID = 2, Name = "p2" };
            //Tạo mock repo
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "p1"},
                delProduct,
                new Product {ProductID = 3, Name = "p3"}
            });

            //Tạo controller
            AdminController adminController = new AdminController(mock.Object);



            //Action
            //Delete product
            adminController.Delete(delProduct.ProductID);


            //Assert
            //ensure that the repository delete method was
            // called with the correct Product
            mock.Verify(m => m.DeleteProduct(delProduct.ProductID));
            

        }
    }
}
