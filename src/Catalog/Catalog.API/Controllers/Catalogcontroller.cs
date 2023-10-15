using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class Catalogcontroller : ControllerBase
    {
        private readonly IProductRepository _repositry;
        private readonly ILogger<Catalogcontroller> _logger;

        public Catalogcontroller(IProductRepository repositry, ILogger<Catalogcontroller> logger)
        {
            _repositry = repositry;
            _logger = logger;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _repositry.GetProducts();
            return Ok(products);
        }


        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _repositry.GetProduct(id);
            if ( product == null )
            {
                //_logger.LogError($"Product with id : {id} , Not Found");
                return NotFound();
            }
            return Ok(product);
        }
        [Route("[action]/{category}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductCategory(string category)
        {
            var products = await _repositry.GetProductByCategory(category);
            return Ok(products);
        }

        [Route("[action]/{name}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByName(string name)
        {
            var products = await _repositry.GetProductByName(name);
            if (!products.Any())
                return NotFound();
            return Ok(products);
        }


        [HttpPut]

        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await _repositry.Update(product));
        }

        [HttpPost]

        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _repositry.Create(product);
            return CreatedAtRoute("GetProduct", new { id = product.Id, }, product);
        }

        [HttpDelete("{id:length(24)}")]

        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteProductById(string id)
        {
            return Ok(await _repositry.Delete(id));
        }

    }
}
