using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Concrete;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Controllers
{
    //Thêm xác thực bằng cách gọi filters
    [Authorize]
    public class AdminController : Controller
    {
        private IProductRepository repository;

        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }
        public ViewResult Index()
        {
            return View(repository.Products);
        }

       
        public ViewResult Edit(int id)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductID == id);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                //xử lý image trước
                if (image != null)
                {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }


                //Save product đã có image
                repository.SaveProduct(product);
                //Chỗ này không thể dùng ViewBag vì RedirectToAction
                //Dùng session data thì mất công xóa
                //==> dùng TempData là tốt nhất
                TempData["Thong bao"] = string
                .Format("{0} has been saveed", product.Name);
            return RedirectToAction("Index");
            }
            else
            {
                //Trường có lỗ gì đó với data (ModelState)
                return View(product);
            }
            
            
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }


        [HttpPost]
        public ActionResult Delete(int productId)
        {
            Product deletedProduct = repository.DeleteProduct(productId);
            if (deletedProduct != null)
            {
                TempData["thong bao"] = string
                    .Format("{0} was deleted", deletedProduct.Name);
                
            }

            return RedirectToAction("Index");
        }
    }
}