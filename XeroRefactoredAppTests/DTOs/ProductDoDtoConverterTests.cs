using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using XeroRefactoredApp.Models;

namespace XeroRefactoredApp.DTOs.Tests
{
    [TestClass()]
    public class ProductDoDtoConverterTests
    {
        ProductDoDtoConverter converter = new ProductDoDtoConverter();
        [TestMethod()]
        public void FromDOTest_WhenModelIsNullReturnsNull()
        {
            ProductDto dto = converter.FromDO(null);
            Assert.IsNull(dto);
        }

        [TestMethod()]
        public void FromDOTest()
        {
            Product model = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Dummy",
                Description = "",
                Price = 199.99M,
                DeliveryPrice = 11.11M
            };
            ProductDto dto = converter.FromDO(model);
            Assert.IsNotNull(dto);
            Assert.AreEqual(model.Id, dto.Id);
            Assert.AreEqual(model.Name, dto.Name);
            Assert.AreEqual(model.Description, dto.Description);
            Assert.AreEqual(model.Price, dto.Price);
            Assert.AreEqual(model.DeliveryPrice, dto.DeliveryPrice);
        }

        [TestMethod()]
        public void ToDoTest_WhenDtoIsNullReturnsNull()
        {
            Product model = converter.ToDO(null);
            Assert.IsNull(model);
        }

        [TestMethod()]
        public void ToDOTest()
        {
            ProductDto dto = new ProductDto
            {
                Id = Guid.NewGuid(),
                Name = "Dummy",
                Description = "",
                Price = 199.99M,
                DeliveryPrice = 11.11M
            };
            Product model = converter.ToDO(dto);
            Assert.IsNotNull(model);
            Assert.AreEqual(dto.Id, model.Id);
            Assert.AreEqual(dto.Name, model.Name);
            Assert.AreEqual(dto.Description, model.Description);
            Assert.AreEqual(dto.Price, model.Price);
            Assert.AreEqual(dto.DeliveryPrice, model.DeliveryPrice);
        }
    }
}