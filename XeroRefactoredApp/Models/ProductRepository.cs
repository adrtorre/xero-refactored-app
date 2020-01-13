using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using XeroRefactoredApp.Exceptions;

namespace XeroRefactoredApp.Models
{
    public class ProductRepository
    {
        private readonly XeroRefactoredAppContext _context;
        public ProductRepository(XeroRefactoredAppContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.Product.ToList();
        }

        public Product GetProduct(Guid id)
        {
            var product = _context.Product.Find(id);
            if (product == null)
            {
                throw new NotFoundException("product with id [" + id + "] does not exist");
            }
            return product;
        }

        public IEnumerable<Product> SearchProductsByName(string name)
        {
            return _context.Product
                .Where(po => po.Name.ToLower().Contains(name.ToLower()))
                .Select(po => po)
                .ToList();
        }

        public void DeleteProduct(Guid id)
        {
            var product = _context.Product.Find(id);
            if (product == null)
            {
                return;
            }
            _context.Product.Remove(product);
            _context.ProductOption.RemoveRange(_context.ProductOption.Where(po => po.ProductId == id));
            _context.SaveChanges();
        }

        public Product CreateProduct(Product product)
        {
            _context.Add(product);
            _context.SaveChanges();
            return product;
        }

        public void UpdateProduct(Product product)
        {
            if (ProductExists(product.Id) == false)
            {
                throw new NotFoundException("product with id [" + product.Id + "] does not exist");
            }
            _context.Entry(product).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    throw new NotFoundException("product with id [" + product.Id + "] does not exist");
                }
                else
                {
                    throw;
                }
            }
        }

        private bool ProductExists(Guid id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
        private bool ProductOptionExists(Guid id)
        {
            return _context.ProductOption.Any(e => e.Id == id);
        }

        public IEnumerable<ProductOption> GetProductOptions(Guid productId)
        {
            if (ProductExists(productId) == false)
            {
                throw new NotFoundException("product with id [" + productId + "] not found");
            }
            return _context.ProductOption
                .Where(po => po.ProductId == productId)
                .Select(po => po)
                .ToList();
        }

        public ProductOption GetProductOption(Guid productId, Guid productOptionId)
        {
            if (ProductExists(productId) == false)
            {
                throw new NotFoundException("product with id [" + productId + "] does not exist");
            }
            return _context.ProductOption
                .Where(po => po.ProductId == productId && po.Id == productOptionId)
                .Select(po => po)
                .FirstOrDefault();
        }

        public ProductOption CreateProductOption(ProductOption productOption)
        {
            if (ProductExists(productOption.ProductId) == false)
            {
                throw new NotFoundException("product with id [" + productOption.ProductId + "] does not exist");
            }
            _context.ProductOption.Add(productOption);
            _context.SaveChanges();
            return productOption;
        }

        public void UpdateProductOption(ProductOption productOption)
        {
            if (ProductExists(productOption.ProductId) == false)
            {
                throw new NotFoundException("product with id [" + productOption.ProductId + "] does not exist");
            }
            if (ProductOptionExists(productOption.Id) == false)
            {
                throw new NotFoundException("product option with id [" + productOption.Id + "] does not exist");
            }
            _context.Entry(productOption).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductOptionExists(productOption.Id))
                {
                    throw new NotFoundException("product option with id [" + productOption.Id + "] does not exist");
                }
                else
                {
                    throw;
                }
            }
        }

        public void DeleteProductOption(Guid productId, Guid productOptionId)
        {
            if (ProductOptionExists(productOptionId))
            {
                ProductOption po = _context.ProductOption.Find(productOptionId);
                if (po != null)
                {
                    if (po.ProductId == productId || ProductExists(po.ProductId) == false)
                    {
                        _context.ProductOption.Remove(po);
                        _context.SaveChanges();
                    }
                }
            }
        }
    }
}
