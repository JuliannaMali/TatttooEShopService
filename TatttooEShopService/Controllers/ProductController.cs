using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TattooEShopApplication.Services;
using TattooEShopDomain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TatttooEShopService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/<ProductController>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = await _productService.GetAllAsync();
            return Ok(result);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var result = await _productService.GetAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST api/<ProductController>
        [Authorize(Policy = "EmployeeOnly")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Product product)
        {
            var result = await _productService.Add(product);

            return Ok(result);
        }

        // PUT api/<ProductController>/5
        [Authorize(Policy = "EmployeeOnly")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Product product)
        {
            var result = await _productService.Update(product);

            return Ok(result);
        }

        // DELETE api/<ProductController>/5
        [Authorize(Policy = "EmployeeOnly")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await _productService.GetAsync(id);
            product.Deleted = true;
            var result = await _productService.Update(product);

            return Ok(result);
        }
    }
}
