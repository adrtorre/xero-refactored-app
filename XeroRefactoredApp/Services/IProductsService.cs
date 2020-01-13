using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeroRefactoredApp.Models;

namespace XeroRefactoredApp.Services
{
    public interface IProductsService
    {
        IEnumerable<Product> GetProducts();

        Product GetProduct(Guid id);

        IEnumerable<Product> SearchProductsByName(string name);

        void DeleteProduct(Guid id);

        Product CreateProduct(Product product);

        void UpdateProduct(Product product);

        IEnumerable<ProductOption> GetProductOptions(Guid productId);

        ProductOption GetProductOption(Guid productId, Guid productOptionId);

        ProductOption CreateProductOption(Guid productId, ProductOption productOption);

        void UpdateProductOption(ProductOption productOption);

        void DeleteProductOption(Guid productId, Guid productOptionId);
    }
}
