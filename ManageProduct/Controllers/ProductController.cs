using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebAPI.Constant;
using TestWebAPI.IServices;
using TestWebAPI.PhanTrang;
using TestWebAPI.Services;

namespace TestWebAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ISanPhamService sanPhamService;

        public ProductController()
        {
            sanPhamService = new ProductServices();
        }

        [HttpPost("ByProductDetail")]
        public IActionResult ByProductDetail([FromQuery] int productDetailId, int quantity)
        {
            var ans = sanPhamService.BuyQuantityProducts(productDetailId, quantity);
            if(ans == ErrorMessage.ThanhCong)
            {
                return Ok("Mua hang thanh cong!!");
            }
            else
            {
                return BadRequest("Mua hang that bai");
            }
        }

        [HttpPost("UpdateProductDetail")]
        public IActionResult UpdateProductDetail([FromQuery] int productDetailId, int quantity)
        {
            var ans = sanPhamService.BuyQuantityProducts(productDetailId, quantity);
            if (ans == ErrorMessage.ThanhCong)
            {
                return Ok("Cap nhat thanh cong!!");
            }
            else
            {
                return BadRequest("Cap nhat that bai");
            }
        }

        [HttpGet("GetListProductDetail")]
        public IActionResult GetListProductDetail([FromQuery] Pagination pagination)
        {
            return Ok(sanPhamService.GetListProductDeTails(pagination));
        }
    }
}
