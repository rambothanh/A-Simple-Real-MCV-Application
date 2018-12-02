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
    }
}
