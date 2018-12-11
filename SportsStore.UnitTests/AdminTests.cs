using System;
using System.Collections.Generic;
using System.Linq;
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
            Assert.AreEqual(result.Length,3);
            Assert.AreEqual(result[0].Name, "p1");
            Assert.AreEqual(result[1].Name, "p2");
            Assert.AreEqual(result[2].Name, "p3");

        }
    }
}
