using AuthServer.Core.DTOs;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Auth.Server.API.Controllers
{
    //Authorize eklenmesinin sebebi ürünlere sadece üye olan kişilerin işlem yapılmasının istenmesidir.
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : CustomBaseController
    {
        private readonly IServiceGeneric<Product,ProductDto> _produckService;

        public ProductController(IServiceGeneric<Product, ProductDto> produckService)
        {
            _produckService = produckService;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return ActionResultInstance(await _produckService.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> SaveProduct(ProductDto productDto)
        {
            return ActionResultInstance(await _produckService.AddAsync(productDto));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto productDto)
        {
            return ActionResultInstance(await _produckService.Update(productDto, productDto.Id));
        }

        //api/product/3
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return ActionResultInstance(await _produckService.Remove(id));
        }
    }
}
