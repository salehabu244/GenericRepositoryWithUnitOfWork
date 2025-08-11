using GenericRepositoryWithUnitOfWork.Entity;
using GenericRepositoryWithUnitOfWork.Repository;
using GenericRepositoryWithUnitOfWork.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GenericRepositoryWithUnitOfWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductUOWController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductUOWController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
           var result = await _unitOfWork.GetRepository<Product>().GetAllAsync();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] RequestProduct product)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransactionAsync();
                var productEntity = new Product
                {
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Qty = product.Qty
                };
                var productResult = await _unitOfWork.GetRepository<Product>().AddAsync(productEntity);
                await _unitOfWork.SaveChangesAsync();
                var orderEntity = new Order
                {
                    ProductId = productResult.Id,
                    OrderTime = DateTime.Now,
                };
                var orderResult = await _unitOfWork.GetRepository<Order>().AddAsync(orderEntity);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return StatusCode((int)HttpStatusCode.Created, new { Id = productResult.Id });

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
