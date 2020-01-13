using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using XeroRefactoredApp.Models;
using XeroRefactoredApp.Exceptions;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace XeroRefactoredApp.Services
{
    public class ProductsService : IProductsService
    {
        private readonly ILogger _logger;
        private readonly ProductRepository _repository;
        public ProductsService(ProductRepository repository, ILogger<ProductsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _repository.GetProducts();
        }

        public Product GetProduct(Guid id)
        {
            try
            {
                return _repository.GetProduct(id);
            } catch (NotFoundException e)
            {
                _logger.LogError(e.Message);
                throw e;
            }
        }

        public IEnumerable<Product> SearchProductsByName(string name)
        {
            if (name == null || name.Trim().Length == 0)
            {
                _logger.LogError("name parameter cannot be null or empty when searching products");
                throw new InvalidArgumentException("name parameter cannot be null or empty when searching products");
            }
            return _repository.SearchProductsByName(name);
        }

        public void DeleteProduct(Guid id)
        {
            _repository.DeleteProduct(id);
        }

        public Product CreateProduct(Product product)
        {
            ValidateProduct(product, true);
            return _repository.CreateProduct(product);
        }

        public void UpdateProduct(Product product)
        {
            ValidateProduct(product, false);
            _repository.UpdateProduct(product);
        }
        
        public IEnumerable<ProductOption> GetProductOptions(Guid productId)
        {
            return _repository.GetProductOptions(productId);
        }

        public ProductOption GetProductOption(Guid productId, Guid productOptionId)
        {
            return _repository.GetProductOption(productId, productOptionId);
        }

        public ProductOption CreateProductOption(Guid productId, ProductOption productOption)
        {
            ValidateProductOption(productOption, true);
            return _repository.CreateProductOption(productOption);
        }

        public void UpdateProductOption(ProductOption productOption)
        {
            ValidateProductOption(productOption, false);
            _repository.UpdateProductOption(productOption);
        }

        public void DeleteProductOption(Guid productId, Guid productOptionId)
        {
            _repository.DeleteProductOption(productId, productOptionId);
        }

        private void ValidateProduct(Product product, bool isCreateAction)
        {
            if (isCreateAction && product.Id != Guid.Empty)
            {
                string message = "product id must not be specified when creating new product, it will be auto-generated";
                _logger.LogError(message);
                throw new InvalidArgumentException(message);
            }
            ValidationContext vc = new ValidationContext(product);
            ICollection<ValidationResult> results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(product, vc, results, true);
            if (isValid == false)
            {
                string message = string.Join("", (from o in results
                                                  select o.ErrorMessage).ToArray());
                _logger.LogError(message);
                throw new InvalidArgumentException(message);
            }
        }

        private void ValidateProductOption(ProductOption productOption, bool isCreateAction)
        {
            if (isCreateAction && productOption.Id != Guid.Empty)
            {
                string message = "product option id must not be specified when creating new product option, it will be auto-generated";
                _logger.LogError(message);
                throw new InvalidArgumentException(message);
            }
            ValidationContext vc = new ValidationContext(productOption);
            ICollection<ValidationResult> results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(productOption, vc, results, true);
            if (isValid == false)
            {
                string message = string.Join("", (from o in results
                                                  select o.ErrorMessage).ToArray());
                _logger.LogError(message);
                throw new InvalidArgumentException(message);
            }
        }
    }
}
