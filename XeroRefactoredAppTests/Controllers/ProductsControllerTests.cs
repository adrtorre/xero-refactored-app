using Microsoft.VisualStudio.TestTools.UnitTesting;
using XeroRefactoredApp.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using XeroRefactoredApp.Services;
using XeroRefactoredApp.Models;
using Moq;
using Microsoft.AspNetCore.Mvc;
using XeroRefactoredApp.DTOs;
using System.Linq;
using XeroRefactoredApp.Exceptions;
using Microsoft.Extensions.Logging;

namespace XeroRefactoredApp.Controllers.Tests
{
    [TestClass()]
    public class ProductsControllerTests
    {
        [TestMethod()]
        public void GetProductsTest()
        {
            List<Product> listOfProducts = new List<Product>();
            listOfProducts.Add(new Product { Id = Guid.NewGuid(), Name = "Samsung Galaxy", Description = "mobile", Price = 199.99M, DeliveryPrice = 11.11M });
            listOfProducts.Add(new Product { Id = Guid.NewGuid(), Name = "Apple iPod", Description = "listen to your music", Price = 299.99M, DeliveryPrice = 11.11M });
            var productsService = new Mock<IProductsService>();
            productsService
                .Setup(repo => repo.GetProducts())
                .Returns(listOfProducts);
            ProductsController productsController = new ProductsController(productsService.Object, Mock.Of<ILogger<ProductsController>>());

            ActionResult result = productsController.GetProducts();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            OkObjectResult okObjectResult = (OkObjectResult)result;
            List<ProductDto> listOfProductsExpected = listOfProducts.Select(p => new ProductDoDtoConverter().FromDO(p)).ToList();
            Assert.IsInstanceOfType(okObjectResult.Value, typeof(List<ProductDto>));
            CollectionAssert.AreEqual((List<ProductDto>) okObjectResult.Value, listOfProductsExpected);
        }

        [TestMethod()]
        public void GetProductTest()
        {
            Guid productId = Guid.NewGuid();
            Product product = new Product { Id = productId, Name = "Samsung Galaxy", Description = "mobile", Price = 199.99M, DeliveryPrice = 11.11M };
            var productsService = new Mock<IProductsService>();
            productsService
                .Setup(repo => repo.GetProduct(productId))
                .Returns(product);
            ProductsController productsController = new ProductsController(productsService.Object, Mock.Of<ILogger<ProductsController>>());

            ActionResult result = productsController.GetProduct(productId);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            OkObjectResult okObjectResult = (OkObjectResult)result;
            Assert.IsInstanceOfType(okObjectResult.Value, typeof(ProductDto));
            ProductDto expectedProduct = new ProductDoDtoConverter().FromDO(product);
            Assert.AreEqual(expectedProduct, (ProductDto) okObjectResult.Value);
        }

        [TestMethod()]
        public void SearchProductsByNameTest()
        {
            Guid productId = Guid.NewGuid();
            List<Product> listOfProducts = new List<Product>();
            listOfProducts.Add(new Product { Id = productId, Name = "Samsung Galaxy", Description = "mobile", Price = 199.99M, DeliveryPrice = 11.11M });
            var productsService = new Mock<IProductsService>();
            productsService
                .Setup(repo => repo.SearchProductsByName("Samsung"))
                .Returns(listOfProducts);
            ProductsController productsController = new ProductsController(productsService.Object, Mock.Of<ILogger<ProductsController>>());

            ActionResult result = productsController.SearchProductsByName("Samsung");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            OkObjectResult okObjectResult = (OkObjectResult)result;
            List<ProductDto> listOfProductsExpected = listOfProducts.Select(p => new ProductDoDtoConverter().FromDO(p)).ToList();
            Assert.IsInstanceOfType(okObjectResult.Value, typeof(List<ProductDto>));
            CollectionAssert.AreEqual((List<ProductDto>)okObjectResult.Value, listOfProductsExpected);
        }

        [TestMethod()]
        public void SearchProductsByNameTest_WhenServiceThrowsInvalidArgumentException()
        {
            string errorMessage = "name parameter cannot be null or empty when searching products";
            var productsService = new Mock<IProductsService>();
            productsService
                .Setup(repo => repo.SearchProductsByName(""))
                .Throws(new InvalidArgumentException(errorMessage));
            ProductsController productsController = new ProductsController(productsService.Object, Mock.Of<ILogger<ProductsController>>());

            ActionResult result = productsController.SearchProductsByName("");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            BadRequestObjectResult badRequestResult = (BadRequestObjectResult) result;
            Assert.AreEqual(errorMessage, (string) badRequestResult.Value);
        }

        [TestMethod()]
        public void PostProductTest()
        {
            ProductDto product = new ProductDto { Name = "Samsung Galaxy", Description = "mobile", Price = 199.99M, DeliveryPrice = 11.11M };
            var productsService = new Mock<IProductsService>();
            Guid productId = Guid.NewGuid();
            Product model = new Product { Id = productId, Name = "Samsung Galaxy", Description = "mobile", Price = 199.99M, DeliveryPrice = 11.11M };
            productsService
                .Setup(repo => repo.CreateProduct(new ProductDoDtoConverter().ToDO(product)))
                .Returns(model);
            ProductsController productsController = new ProductsController(productsService.Object, Mock.Of<ILogger<ProductsController>>());

            ActionResult result = productsController.PostProduct(product);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            CreatedAtActionResult createdAtActionResult = (CreatedAtActionResult) result;
            Assert.IsInstanceOfType(createdAtActionResult.Value, typeof(ProductDto));
            ProductDto expectedProduct = new ProductDoDtoConverter().FromDO(model);
            Assert.AreEqual(expectedProduct, (ProductDto)createdAtActionResult.Value);
            object obj;
            bool contains = createdAtActionResult.RouteValues.TryGetValue("id", out obj);
            Assert.AreEqual(true, contains);
            Assert.AreEqual(productId, obj);
        }

        public void PostProductTest_WhenServiceThrowsInvalidArgumentException()
        {
            ProductDto product = new ProductDto { Description = "mobile", Price = 199.99M, DeliveryPrice = 11.11M };
            var productsService = new Mock<IProductsService>();
            productsService
                .Setup(repo => repo.CreateProduct(new ProductDoDtoConverter().ToDO(product)))
                .Throws(new InvalidArgumentException("Name is required"));
            ProductsController productsController = new ProductsController(productsService.Object, Mock.Of<ILogger<ProductsController>>());

            ActionResult result = productsController.PostProduct(product);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            BadRequestObjectResult badRequestResult = (BadRequestObjectResult)result;
            Assert.AreEqual("Name is required", (string)badRequestResult.Value);
        }

        [TestMethod()]
        public void PutProductTest()
        {
            Guid productId = Guid.NewGuid();
            ProductDto product = new ProductDto { Id = productId, Name = "Samsung Galaxy", Description = "mobile", Price = 199.99M, DeliveryPrice = 11.11M };
            var productsService = new Mock<IProductsService>();
            productsService
                .Setup(repo => repo.UpdateProduct(new ProductDoDtoConverter().ToDO(product)));
            ProductsController productsController = new ProductsController(productsService.Object, Mock.Of<ILogger<ProductsController>>());

            ActionResult result = productsController.PutProduct(productId, product);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod()]
        public void PutProductTest_WithInvalidArgument()
        {
            Guid productId = Guid.NewGuid();
            ProductDto product = new ProductDto { Id = productId, Name = "Samsung Galaxy", Description = "mobile", Price = 199.99M, DeliveryPrice = 11.11M };
            var productsService = new Mock<IProductsService>();
            productsService
                .Setup(repo => repo.UpdateProduct(new ProductDoDtoConverter().ToDO(product)));
            ProductsController productsController = new ProductsController(productsService.Object, Mock.Of<ILogger<ProductsController>>());

            Guid inputProdId = Guid.NewGuid();
            ActionResult result = productsController.PutProduct(inputProdId, product);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            BadRequestObjectResult badRequestResult = (BadRequestObjectResult)result;
            Assert.AreEqual("product id [" + inputProdId + "] does not match id provided inside the product request [" + productId + "]", (string)badRequestResult.Value);
        }


        [TestMethod()]
        public void PutProductTest_WhenProductDoesNotExist()
        {
            Guid productId = Guid.NewGuid();
            ProductDto product = new ProductDto { Id = productId, Name = "Samsung Galaxy", Description = "mobile", Price = 199.99M, DeliveryPrice = 11.11M };
            var productsService = new Mock<IProductsService>();
            productsService
                .Setup(repo => repo.UpdateProduct(new ProductDoDtoConverter().ToDO(product)))
                .Throws(new NotFoundException("product with id [" + product.Id + "] does not exist"));
            ProductsController productsController = new ProductsController(productsService.Object, Mock.Of<ILogger<ProductsController>>());

            ActionResult result = productsController.PutProduct(productId, product);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            NotFoundObjectResult notFoundObjectResult = (NotFoundObjectResult) result;
            Assert.AreEqual("product with id [" + product.Id + "] does not exist", (string)notFoundObjectResult.Value);
        }

        [TestMethod()]
        public void DeleteProductTest()
        {
            Guid productId = Guid.NewGuid();
            var productsService = new Mock<IProductsService>();
            productsService
                .Setup(repo => repo.DeleteProduct(productId));
            ProductsController productsController = new ProductsController(productsService.Object, Mock.Of<ILogger<ProductsController>>());

            ActionResult result = productsController.DeleteProduct(productId);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod()]
        public void GetProductOptionsTest()
        {
            Guid productId = Guid.NewGuid();
            List<ProductOption> listOfProductOptions = new List<ProductOption>();
            listOfProductOptions.Add(new ProductOption { Id = Guid.NewGuid(), ProductId = productId, Name = "OptionA", Description = "with keyboard" });
            listOfProductOptions.Add(new ProductOption { Id = Guid.NewGuid(), ProductId = productId, Name = "OptionB", Description = "additional SD card" });
            var productsService = new Mock<IProductsService>();
            productsService
                .Setup(repo => repo.GetProductOptions(productId))
                .Returns(listOfProductOptions);
            ProductsController productsController = new ProductsController(productsService.Object, Mock.Of<ILogger<ProductsController>>());

            ActionResult result = productsController.GetProductOptions(productId);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            OkObjectResult okObjectResult = (OkObjectResult)result;
            List<ProductOptionDto> listOfProductsExpected = listOfProductOptions.Select(p => new ProductOptionDoDtoConverter().FromDO(p)).ToList();
            Assert.IsInstanceOfType(okObjectResult.Value, typeof(List<ProductOptionDto>));
            CollectionAssert.AreEqual(listOfProductsExpected, (List<ProductOptionDto>)okObjectResult.Value);
        }

        [TestMethod()]
        public void GetProductOptionTest()
        {
            Guid productId = Guid.NewGuid();
            Guid productOptionId = Guid.NewGuid();
            ProductOption productOption = new ProductOption { Id = productOptionId, ProductId = productId, Name = "OptionA", Description = "with keyboard" };
            var productsService = new Mock<IProductsService>();
            productsService
                .Setup(repo => repo.GetProductOption(productId, productOptionId))
                .Returns(productOption);
            ProductsController productsController = new ProductsController(productsService.Object, Mock.Of<ILogger<ProductsController>>());

            ActionResult result = productsController.GetProductOption(productId, productOptionId);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            OkObjectResult okObjectResult = (OkObjectResult) result;
            ProductOptionDto productOptionDtoExpected = new ProductOptionDoDtoConverter().FromDO(productOption);
            Assert.IsInstanceOfType(okObjectResult.Value, typeof(ProductOptionDto));
            Assert.AreEqual(productOptionDtoExpected, (ProductOptionDto) okObjectResult.Value);
        }

        [TestMethod()]
        public void PostProductOptionTest()
        {
            Guid productId = Guid.NewGuid();
            ProductOptionDto productOption = new ProductOptionDto { Name = "OptionA", Description = "desc", ProductId = productId };
            ProductOption model = new ProductOption { Id = Guid.NewGuid(), Name = "OptionA", Description = "desc", ProductId = productId };
            var productsService = new Mock<IProductsService>();
            productsService
                .Setup(repo => repo.CreateProductOption(productId, new ProductOptionDoDtoConverter().ToDO(productOption)))
                .Returns(model);
            ProductsController productsController = new ProductsController(productsService.Object, Mock.Of<ILogger<ProductsController>>());

            ActionResult result = productsController.PostProductOption(productId, productOption);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            CreatedAtActionResult createdAtActionResult = (CreatedAtActionResult)result;
            Assert.IsInstanceOfType(createdAtActionResult.Value, typeof(ProductOptionDto));
            ProductOptionDto expectedProduct = new ProductOptionDoDtoConverter().FromDO(model);
            Assert.AreEqual(expectedProduct, (ProductOptionDto) createdAtActionResult.Value);
            object productIdActual;
            bool contains = createdAtActionResult.RouteValues.TryGetValue("productId", out productIdActual);
            Assert.AreEqual(true, contains);
            Assert.AreEqual(productId, productIdActual);
            object productOptionId;
            contains = createdAtActionResult.RouteValues.TryGetValue("productOptionId", out productOptionId);
            Assert.AreEqual(true, contains);
            Assert.AreEqual(model.Id, productOptionId);
        }

        [TestMethod()]
        public void PutProductOptionTest()
        {
            Guid productId = Guid.NewGuid();
            Guid productOptionId = Guid.NewGuid();
            ProductOptionDto productOption = new ProductOptionDto { Id = productOptionId, Name = "OptionA", Description = "desc", ProductId = productId };
            var productsService = new Mock<IProductsService>();
            productsService
                .Setup(repo => repo.UpdateProductOption(new ProductOptionDoDtoConverter().ToDO(productOption)));
            ProductsController productsController = new ProductsController(productsService.Object, Mock.Of<ILogger<ProductsController>>());

            ActionResult result = productsController.PutProductOption(productId, productOptionId, productOption);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod()]
        public void DeleteProductOptionTest()
        {
            Guid productId = Guid.NewGuid();
            Guid productOptionId = Guid.NewGuid();
            var productsService = new Mock<IProductsService>();
            productsService
                .Setup(repo => repo.DeleteProductOption(productId, productOptionId));
            ProductsController productsController = new ProductsController(productsService.Object, Mock.Of<ILogger<ProductsController>>());

            ActionResult result = productsController.DeleteProductOption(productId, productOptionId);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}