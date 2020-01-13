using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using XeroRefactoredApp.Models;
using XeroRefactoredApp.Services;
using XeroRefactoredApp.Exceptions;
using XeroRefactoredApp.DTOs;
using Microsoft.Extensions.Logging;

namespace XeroRefactoredApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IProductsService _productsService;
        private readonly ProductDoDtoConverter _productConverter;
        private readonly ProductOptionDoDtoConverter _productOptionConverter;

        public ProductsController(IProductsService productsService, ILogger<ProductsController> logger)
        {
            _productsService = productsService;
            _productConverter = new ProductDoDtoConverter();
            _productOptionConverter = new ProductOptionDoDtoConverter();
            _logger = logger;
        }

        // GET: Products
        [HttpGet]
        public ActionResult GetProducts()
        {
            return Ok(_productsService.GetProducts()
                .Select(p => _productConverter.FromDO(p))
                .ToList());
        }

        // GET: Products/5
        [HttpGet("{id}")]
        public ActionResult GetProduct(Guid id)
        {
            try
            {
                Product product = _productsService.GetProduct(id);
                return Ok(_productConverter.FromDO(product));
            } catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        // GET: Products?name=xyz
        [HttpGet("_search")]
        public ActionResult SearchProductsByName(string name)
        {
            try
            {
                return Ok(_productsService.SearchProductsByName(name)
                .Select(p => _productConverter.FromDO(p))
                .ToList());
            } catch (InvalidArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: Products
        [HttpPost]
        public ActionResult PostProduct(ProductDto product)
        {
            try
            {
                product = _productConverter.FromDO(_productsService.CreateProduct(_productConverter.ToDO(product)));
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            } catch (InvalidArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: Products/5
        [HttpPut("{id}")]
        public ActionResult PutProduct(Guid id, ProductDto product)
        {
            if (id != product.Id)
            {
                string message = "product id [" + id + "] does not match id provided inside the product request [" + product.Id + "]";
                _logger.LogError(message);
                return BadRequest(message);
            }
            try
            {
                _productsService.UpdateProduct(_productConverter.ToDO(product));
            }
            catch (InvalidArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            return NoContent();
        }

        // DELETE: Products/5
        [HttpDelete("{id}")]
        public ActionResult DeleteProduct(Guid id)
        {
            _productsService.DeleteProduct(id);
            return NoContent();
        }

        /* product options  */

        // GET: Products/1/options
        [HttpGet("{productId}/options")]
        public ActionResult GetProductOptions(Guid productId)
        {
            try
            {
                return Ok(_productsService.GetProductOptions(productId)
                    .Select(po => _productOptionConverter.FromDO(po))
                    .ToList());
            } catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        // GET: Products/1/options/11
        [HttpGet("{productId}/options/{productOptionId}")]
        public ActionResult GetProductOption(Guid productId, Guid productOptionId)
        {
            try
            {
                ProductOptionDto po = _productOptionConverter.FromDO(_productsService.GetProductOption(productId, productOptionId));
                if (po == null)
                {
                    return NotFound("product option with id [" + productOptionId + "] not found");
                }
                return Ok(po);
            } catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        // POST: Products/1/options
        [HttpPost("{productId}/options")]
        public ActionResult PostProductOption(Guid productId, ProductOptionDto productOption)
        {
            try
            {
                productOption.ProductId = productId;
                productOption = _productOptionConverter.FromDO(_productsService.CreateProductOption(productId, _productOptionConverter.ToDO(productOption)));
                return CreatedAtAction("GetProductOption", new { productId = productOption.ProductId, productOptionId = productOption.Id }, productOption);
            } catch (InvalidArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: Products/1/options/11
        [HttpPut("{productId}/options/{productOptionId}")]
        public ActionResult PutProductOption(Guid productId, Guid productOptionId, ProductOptionDto productOption)
        {

            if (productId != productOption.ProductId)
            {
                string message = "product id [" + productId + "] does not match ProductId provided inside the product option request [" + productOption.ProductId + "]";
                _logger.LogError(message);
                return BadRequest(message);
            }
            if (productOptionId != productOption.Id)
            {
                string message = "product option id [" + productOptionId + "] does not match Id provided inside the product option request [" + productOption.Id + "]";
                _logger.LogError(message);
                return BadRequest(message);
            }
            try
            {
                _productsService.UpdateProductOption(_productOptionConverter.ToDO(productOption));
            }
            catch (InvalidArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            return NoContent();
        }

        // DELETE: Products/1/options/11
        [HttpDelete("{productId}/options/{productOptionId}")]
        public ActionResult DeleteProductOption(Guid productId, Guid productOptionId)
        {
            _productsService.DeleteProductOption(productId, productOptionId);
            return NoContent();
        }
    }
}
