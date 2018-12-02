using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        //Tạo đối tượng kiểu IProductRepository sẽ tự 
        //thêm một đối tượng khi chạy hàm khởi tạo
        //như bên dưới , đối tượng này chính là những
        //dữ liệu để test đơn giản mà ta đã chỉ định ở 
        //file NinjectDependencyResolver.cs, hoặc đó là 
        //một thể hiện của lớp EFProductRepository (tùy 
        //theo đã chỉ định như thế nào)
        private IProductRepository repository;

        //Chỉ định 4 sản phẩm trên 1 trang
        public int PageSize = 4;

        public ProductController(IProductRepository productRepository)
        {
            this.repository = productRepository;
        }

        //Hàm List có tham số mắc định page =1
        public ViewResult List(int page = 1)
        {

            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products
                    .OrderBy(p => p.ProductID)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Products.Count()
                }
            };
            return View(model);

            //return View(repository.Products
            //    .OrderBy(p=>p.ProductID)
            //    .Skip((page-1)*PageSize)
            //    .Take(PageSize));

            ////.Skip((page-1)*PageSize): bỏ qua các product ở phía trước page
            ////trường hợp tham số mặc định page =1 thì không làm gì hết
            ////.Take(PageSize) lấy 4 product đầu tiên của vùng còn lại sau khi
            ////đã dùng Skip
            ////thêm "?page=2" trên url để điều hướng đến trang 2 của Product


        }

    }




}

