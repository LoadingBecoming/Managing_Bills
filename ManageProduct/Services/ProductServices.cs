using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebAPI.Constant;
using TestWebAPI.Entities;
using TestWebAPI.IServices;
using TestWebAPI.PhanTrang;

namespace TestWebAPI.Services
{
    public class ProductServices : ISanPhamService
    {
        AppDbContext DbContext;

        public ProductServices()
        {
            DbContext = new AppDbContext();
        }

        public ErrorMessage BuyQuantityProducts(int productDetailId, int quantity)
        {
            ProductDetails productDetails = GetProductDetail(productDetailId);
                                                
            int quantityOfProduct = (int)productDetails.Quantity;
            if (quantityOfProduct == 0)
            {
                return ErrorMessage.HetHang;
            }
            else if (quantityOfProduct > 0 && quantityOfProduct < quantity)
            {
                return ErrorMessage.KhongDu;
            }
            else
            {
                quantityOfProduct -= quantity;
                productDetails.Quantity = quantityOfProduct;
                DbContext.Update(productDetails);
                DbContext.SaveChanges();
                return ErrorMessage.ThanhCong;
            }
        }

        private ProductDetails GetProductDetail(int productDetailId)
        {
            return DbContext.ProductDetails.FirstOrDefault(x => x.ProductDetailId == productDetailId);
        }

        public ErrorMessage UpdateQuantityProducts(int productDetailId, int quantity)
        {
            ProductDetails productDetails = GetProductDetail(productDetailId);

            if (quantity < 0)
            {
                return ErrorMessage.ThatBai;
            }
            else
            {
                productDetails.Quantity = quantity;
                DbContext.Update(productDetails);
                DbContext.SaveChanges();
                return ErrorMessage.ThanhCong;
            }
        }

        public PageResult<ProductDetails> GetListProductDeTails(Pagination pagination)
        {
            var ListPrDetail =  DbContext.ProductDetails.AsQueryable();
            var result = PageResult<ProductDetails>.ToPageRusult(pagination, ListPrDetail);
            pagination.TotalCount = ListPrDetail.Count();
            return new PageResult<ProductDetails>(pagination, result);
        }
    }


}
