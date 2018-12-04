using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
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
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

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
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

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
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

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
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 10M};
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 20M};

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
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 10M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 20M };

            Cart cart = new Cart();


            cart.AddItem(p1, 2);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 5);
            //act
            cart.Clear();

            //assert
            Assert.AreEqual(cart.Lines.Count(), 0);

        }
    }
}
