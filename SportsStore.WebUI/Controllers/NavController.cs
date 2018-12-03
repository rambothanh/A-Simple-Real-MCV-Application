﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;

namespace SportsStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IProductRepository repository;

        public NavController(IProductRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu(string category = null)
           

        {
            //Tạo cate đang được chọn: sẽ nhận tự động từ parameter của 
            //phương thức Menu (đã cấu hình router)
            ViewBag.SelectedCategory = category;


            IEnumerable<string> categories = repository.Products
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x);
            //.Distinct(): loại bỏ duplicates

            return PartialView(categories);
        }
    }
}