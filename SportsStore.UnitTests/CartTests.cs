using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using static SportsStore.Domain.Entities.Cart;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_LineCart()
        {
            //arrange
            Product p1 = new Product {ProductID = 1, Name = "P1"};
            Product p2 = new Product {ProductID = 2, Name = "P2"};

            Cart cart = new Cart();

            //act
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 2);
            CartLine[] results = cart.Lines.ToArray();

            //assert
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Product, p1);
            Assert.AreEqual(results[1].Product, p2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //arrange
            Product p1 = new Product {ProductID = 1, Name = "P1"};
            Product p2 = new Product {ProductID = 2, Name = "P2"};

            Cart cart = new Cart();

            cart.AddItem(p1, 2);
            cart.AddItem(p2, 1);

            //act
            cart.AddItem(p1, 5);
            CartLine[] results = cart.Lines.ToArray();

            //assert
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Quantity, 7);
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            //arrange
            Product p1 = new Product {ProductID = 1, Name = "P1"};
            Product p2 = new Product {ProductID = 2, Name = "P2"};

            Cart cart = new Cart();


            cart.AddItem(p1, 2);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 5);
            //act
            cart.RemoveLine(p1);

            //assert
            Assert.AreEqual(cart.Lines.Count(l => l.Product == p1), 0);
            Assert.AreEqual(cart.Lines.Count(), 1);
        }

        [TestMethod]
        public void Can_Calculate_Cart_Total()
        {
            //arrange
            Product p1 = new Product {ProductID = 1, Name = "P1", Price = 10M};
            Product p2 = new Product {ProductID = 2, Name = "P2", Price = 20M};

            Cart cart = new Cart();


            cart.AddItem(p1, 2);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 5);
            //act
            Decimal result = cart.ComputeTotalValue();

            //assert
            Assert.AreEqual(result, 90M);

        }

        [TestMethod]
        public void Can_Clear_Cart()
        {
            //arrange
            Product p1 = new Product {ProductID = 1, Name = "P1", Price = 10M};
            Product p2 = new Product {ProductID = 2, Name = "P2", Price = 20M};

            Cart cart = new Cart();


            cart.AddItem(p1, 2);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 5);
            //act
            cart.Clear();

            //assert
            Assert.AreEqual(cart.Lines.Count(), 0);

        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
            }.AsQueryable());

            Cart cart = new Cart();
            CartController cartController = new CartController(mock.Object,null);

            //Action
            cartController.AddToCart(cart, 1, null);

            //Assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);

        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Index_View()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "cat1"}
            }.AsQueryable);

            Cart cart = new Cart();
            CartController cartController = new CartController(mock.Object, null);

            //Act
            RedirectToRouteResult result = cartController
                .AddToCart(cart, 2, "myUrl");

            //Assert
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");


        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Arrange - create a Cart
            Cart cart = new Cart();
            // Arrange - create the controller
            CartController cartController = new CartController(null, null);

            // Act - call the Index action method
            CartIndexViewModel result
                = (CartIndexViewModel)cartController.Index(cart, "myUrl").ViewData.Model;
            // Assert
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");


        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Arrange - create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            // Arrange - create an empty cart
            Cart cart = new Cart();
            // Arrange - create shipping details
            ShippingDetails shippingDetails = new ShippingDetails();
            // Arrange - create an instance of the controller
            CartController cartController = new CartController(null, mock.Object);
            // Act
            ViewResult result = cartController.Checkout(cart, shippingDetails);


            // Assert - Kiểm tra order chưa được gửi vào process
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Assert - Kiểm tra phương thức trả về View chính xác
            Assert.AreEqual("", result.ViewName);

            // Assert - check IsValid
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Arrange
            // create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            //Tạo một cart với 1 Item
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            //Tạo một instance của contrller
            CartController cartController = new CartController(null, mock.Object);

            //Thêm một error vào model
            cartController.ModelState.AddModelError("error","error test");

            //Act
            //Thử check out
            ViewResult result = cartController.Checkout(cart, new ShippingDetails());

            //Assert
            //Kiểm tra order chưa được đưa đến processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(),
                It.IsAny<ShippingDetails>()),Times.Never());
            //Kiểm tra phương thức trả về đúng View default
            Assert.AreEqual(result.ViewName,"");

            //Kiểm tra IsValid
            Assert.AreEqual(result.ViewData.ModelState.IsValid,false);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            // Arrange - create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            // Arrange - create a cart with an item
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            // Arrange - create an instance of the controller
            CartController target = new CartController(null, mock.Object);


            // Act - try to checkout
            ViewResult result = target.Checkout(cart, new ShippingDetails());


            // Assert - check that the order has been passed on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Once());
            // Assert - check that the method is returning the Completed view
            Assert.AreEqual("Completed", result.ViewName);
            // Assert - check that I am passing a valid model to the view
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }

    }


}
