using GenericRepositoryWithUnitOfWork.Entity;
using GenericRepositoryWithUnitOfWork.Repository.Interface;
using GenericRepositoryWithUnitOfWork.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenericRepositoryWithUnitOfWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _productRepository;
        public ProductController(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] RequestProduct product)
        {
            if (product == null)
            {
                return BadRequest("Product cannot be null");
            }
            var productEntity = new Product
            {
                ProductName = product.ProductName,
                Price = product.Price,
                Qty = product.Qty
            };
            var createdProduct = await _productRepository.AddAsync(productEntity);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] RequestProduct product)
        {
            var productEntity = await _productRepository.GetByIdAsync(id);
            if (product == null || id != product.Id)
            {
                return BadRequest("Product data is invalid");
            }
            
            if (productEntity == null)
            {
                return NotFound();
            }
            productEntity.ProductName = product.ProductName;
            productEntity.Price = product.Price;
            productEntity.Qty = product.Qty;
            await _productRepository.UpdateAsync(productEntity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var productEntity = await _productRepository.GetByIdAsync(id);
            if (productEntity == null)
            {
                return NotFound();
            }
            await _productRepository.DeleteEntity(productEntity);
            return NoContent();
        }
    }
}
