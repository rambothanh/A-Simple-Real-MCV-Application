using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Concrete
{
    //tạo một lớp có nguồn gốc từ System.Data.Entity.DbContext
    // Lớp này sau đó tự động định nghĩa một thuộc tính cho mỗi
    // bảng trong cơ sở dữ liệu mà ta muốn làm việc

    public class EFDbContext : DbContext
    {
        //The name of the property specifies the table: (trường hợp này là Products)
        //The type parameter of the DbSet result specifies the "model type"
        //"model type" để EF dùng để biểu diễn các hàng trong bảng (trường hợp này là Product)
        public DbSet<Product> Products { get; set; }

        //Nghĩa là: EF phải sử dụng loại Mô hình Product để biểu thị
        //các hàng trong bảng Products.

    }
}
