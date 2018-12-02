using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        //Tạo đối tượng kiểu IProductRepository sẽ tự 
        //thêm một đối tượng giả khi chạy hàm khởi tạo
        //như bên dưới , đối tượng giả này chính là những
        //dữ liệu để test đơn giản mà ta đã chỉ định ở 
        //file NinjectDependencyResolver.cs
        private IProductRepository repository;
        public ProductController(IProductRepository productRepository)
        {
            this.repository = productRepository;
        }
        public ViewResult List()
        {
            return View(repository.Products);
        }
    }
}