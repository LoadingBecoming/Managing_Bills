using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebAPI.Constant;
using TestWebAPI.Entities;
using TestWebAPI.PhanTrang;

namespace TestWebAPI.IServices
{
    interface ISanPhamService
    {
        public ErrorMessage BuyQuantityProducts(int productDetailId, int quantity);
        public ErrorMessage UpdateQuantityProducts(int productDetailId, int quantity);
        public PageResult<ProductDetails> GetListProductDeTails(Pagination pagination);
    }
}
