using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminSecurityTests
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            //Arrange
            //Tạo một mock authentication provider
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "123456")
            ).Returns(true);
            
            //Tạo một View model
            LoginViewModel loginViewModel = new LoginViewModel
            {
                UserName = "admin",
                Password = "123456"
            };

            //Tạo cotroller
            AccountController accountController = new AccountController(mock.Object);

            //Action
            //Xác nhận người dùng thông qua thông tin hợp lệ
            ActionResult result = accountController.Login(loginViewModel, "/TestUrl");
            
            //Assert
            Assert.IsInstanceOfType(result,typeof(RedirectResult));
            Assert.AreEqual(((RedirectResult)result).Url, "/TestUrl");
        }

        [TestMethod]
        public void Cannot_Login_With_InValid_Credentials()
        {
            //Arrange
            //Tạo một mock authentication provider
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "123456")
            ).Returns(false);

            //Tạo một View model
            LoginViewModel loginViewModel = new LoginViewModel
            {
                UserName = "admin",
                Password = "123456"
            };

            //Tạo cotroller
            AccountController accountController = new AccountController(mock.Object);

            //Action
            //Xác nhận người dùng thông qua thông tin hợp lệ
            ActionResult result = accountController.Login(loginViewModel, "/TestUrl");

            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
