using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        private EFDbContext context = new EFDbContext();

        // implements the IProductRepository interface
        public IEnumerable<Product> Products
        {
            get { return context.Products; }
        }
        //sử dụng một thể hiện của EFDbContext để lấy dữ liệu từ database
        //bằng cách sử dụng EF

        //implements a method
        public void SaveProduct(Product product)
        {
            //Nếu chưa có trong database thì thêm vào
            if (product.ProductID ==0)
            {
                context.Products.Add(product);
            }
            //Nếu đã tồn tại trong database thì chỉnh sửa
            else
            {
                Product dbEntry = context.Products.Find(product.ProductID);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;

                }
            }

            context.SaveChanges();
        }
    }
}
