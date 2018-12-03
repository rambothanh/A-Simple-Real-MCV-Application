using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            });
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Act
            
            ProductsListViewModel result =
                (ProductsListViewModel)controller.List(null,2).Model;
            

            // Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //Arrange - define an HTML helper
            //we need to do this in order to apply the extension method
            HtmlHelper myHelper = null;
            //Tạo data cho Paginginfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                ItemsPerPage = 10,
                TotalItems = 28
            };

            //Cài đặt delegate sử dụng lambda expression
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            //Act
            //Chạy extenstion method PageLinks của lớp PagingHelpers
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            //Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                            + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                            + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());


        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            });
            // Arrange
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            // Act
            ProductsListViewModel result = (ProductsListViewModel)controller
                .List(null,2).Model;
            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            //Arrange
            //Tạo Mock
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1", Category = "cat1"},
                new Product {ProductID = 2, Name = "P2",Category = "cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "cat1"},
                new Product {ProductID = 4, Name = "P4",Category = "cat3"},
                new Product {ProductID = 5, Name = "P5",Category = "cat2"}
            });
            // Arrange - create a controller and make the page size 3 items
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Action
            Product[] result = ((ProductsListViewModel)controller
                .List("cat2", 1).Model).Products.ToArray();

            //Assert
           Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name=="P2" && result[0].Category == "cat2");
            Assert.IsTrue(result[1].Name == "P5" && result[1].Category == "cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            //Arrange
            //Tạo Mock
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1", Category = "cat1"},
                new Product {ProductID = 2, Name = "P2",Category = "cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "cat1"},
                new Product {ProductID = 4, Name = "P4",Category = "cat3"},
                new Product {ProductID = 5, Name = "P5",Category = "cat2"}
            });
            // Arrange 
            NavController controller = new NavController(mock.Object);
            
            //Action
            string[] result = ((IEnumerable<string>)controller.Menu().Model).ToArray();

            //Assert
            Assert.AreEqual(result.Length, 3);
            Assert.IsTrue(result[0] == "cat1" );
            Assert.IsTrue(result[1] == "cat2");
            Assert.IsTrue(result[2] == "cat3");

        }

        [TestMethod]
        public void Can_Selected_Catgory()
        {
            //Arrange
            //Tạo Mock
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1", Category = "cat1"},
                new Product {ProductID = 2, Name = "P2",Category = "cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "cat1"},
                new Product {ProductID = 4, Name = "P4",Category = "cat3"},
                new Product {ProductID = 5, Name = "P5",Category = "cat2"}
            });
            // Arrange 
            NavController controller = new NavController(mock.Object);
            var categoryToSelect = "cat2";

            //Action
            string result = controller.Menu(categoryToSelect).ViewBag.SelectedCategory;

            //Assert
            Assert.AreEqual(categoryToSelect, result);


        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            //Arrange
            //Tạo Mock
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1", Category = "cat1"},
                new Product {ProductID = 2, Name = "P2",Category = "cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "cat1"},
                new Product {ProductID = 4, Name = "P4",Category = "cat3"},
                new Product {ProductID = 5, Name = "P5",Category = "cat2"},
                new Product {ProductID = 6, Name = "P6",Category = "cat1"}
            });
            // Arrange 
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Action
           
            int resultCat1 = ((ProductsListViewModel)controller.List("cat1").Model)
                .PagingInfo.TotalItems;
            int resultCat2 = ((ProductsListViewModel)controller.List("cat2").Model)
                .PagingInfo.TotalItems;
            int resultCat3 = ((ProductsListViewModel)controller.List("cat3").Model)
                .PagingInfo.TotalItems;
            int resultAll = ((ProductsListViewModel)controller.List(null).Model)
                .PagingInfo.TotalItems;


            //Assert
          
            Assert.AreEqual(resultCat1, 3);
            Assert.AreEqual(resultCat2, 2);
            Assert.AreEqual(resultCat3, 1);
            Assert.AreEqual(resultAll, 6);


        }


    }
}
