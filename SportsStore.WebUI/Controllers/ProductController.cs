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
        //thêm mội đối tượng giả khi chạy hàm khởi tạo
        //như bên dưới
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