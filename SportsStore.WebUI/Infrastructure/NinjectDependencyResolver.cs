using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moq;
using Ninject;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

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
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product> {
                new Product { Name = "Football", Price = 25 },
                new Product { Name = "Surf board", Price = 179 },
                new Product { Name = "Running shoes", Price = 95 }
            });

            //Ninject trả về cùng một đối tượng giả bất cứ khi nào
            //nó nhận được yêu cầu thực hiện giao diện IProductRepository
            kernel.Bind<IProductRepository>().ToConstant(mock.Object);

            //Thay vì tạo một thể hiện mới của đối tượng implementation mỗi lần
            //,Ninject sẽ luôn đáp ứng các yêu cầu cho interface IProductRepository
            //với cùng một đối tượng giả lập.
        }
    }
}