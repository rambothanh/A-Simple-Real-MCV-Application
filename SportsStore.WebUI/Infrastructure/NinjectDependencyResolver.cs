using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moq;
using Ninject;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Concrete;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Infrastructure.Concrete;

namespace SportsStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            // put bindings here
            //Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //mock.Setup(m => m.Products).Returns(new List<Product> {
            //    new Product { Name = "Football", Price = 25 },
            //    new Product { Name = "Surf board", Price = 179 },
            //    new Product { Name = "Running shoes", Price = 95 }
            //});

            ////Ninject trả về cùng một đối tượng giả bất cứ khi nào
            ////nó nhận được yêu cầu implementation lớp interface IProductRepository
            //kernel.Bind<IProductRepository>().ToConstant(mock.Object);

            ////Thay vì tạo một thể hiện mới của đối tượng implementation mỗi lần
            ////,Ninject sẽ luôn đáp ứng các yêu cầu cho interface IProductRepository
            ////với cùng một đối tượng giả lập.

            kernel.Bind<IProductRepository>().To<EFProductRepository>();
            //Dòng bên trên cho Ninject tạo ra các cá thể của lớp
            //EFProductRepository để phục vụ các requests đến interface IProductRepository.
            //-------------------------------------------
            
            //-------------------------------------------
            //Tạo một đối tượng EmailSettings để sử dụng với phương
            //thức Ninject WithConstructorArgument, mục đích là đưa nó vào hàm
            //tạo EmailOrderProcessor khi các phiên bản mới được tạo cho các yêu
            //cầu dịch vụ cho interface IOrderProcessor.
           
            //Thuộc tính ConfigurationManager.AppSettings đọc giá trị của 
            //"Email.WriteAsFile" trong file Web.config
            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager
                    .AppSettings["Email.WriteAsFile"] ?? "false")
            };


            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("settings", emailSettings);

            //-----------------------------------------------------------------

            kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();

        }
    }
}