using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Entities
{
    public class Cart
    {
        List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Product product, int quantity)
        {
            //Lấy CartLine.Product được chọn (đầu tiên) trong List lineCollection
            CartLine line = lineCollection
                .FirstOrDefault(cartli => cartli.Product.ProductID == product.ProductID);
            //Nếu line = null tức không tìm thấy product trong lineCollection
            // Thì add Product vào và thêm quantity luôn
            //Nếu line != null tức đã có product rồi nên chỉ thêm quantity
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
            
        }

        public void RemoveLine(Product product)
        {
            lineCollection.RemoveAll(m => m.Product.ProductID == product.ProductID);
        }

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(s=> s.Product.Price * s.Quantity);
        }

        public void Clear()
        {
            lineCollection.Clear();
        }

        //Thuuộc tính chỉ trả về lineCollection

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }


        public class CartLine
        {
            public Product Product { get; set; }
            public int Quantity { get; set; }
        }
    }
}
