using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using XeroRefactoredApp.Models;

namespace XeroRefactoredApp.DTOs.Tests
{
    [TestClass()]
    public class ProductOptionDoDtoConverterTests
    {
        ProductOptionDoDtoConverter converter = new ProductOptionDoDtoConverter();
        [TestMethod()]
        public void FromDOTest_WhenModelIsNullReturnsNull()
        {
            ProductOptionDto dto = converter.FromDO(null);
            Assert.IsNull(dto);
        }

        [TestMethod()]
        public void FromDOTest()
        {
            ProductOption model = new ProductOption
            {
                Id = Guid.NewGuid(),
                Name = "Dummy",
                Description = ""
            };
            ProductOptionDto dto = converter.FromDO(model);
            Assert.IsNotNull(dto);
            Assert.AreEqual(model.Id, dto.Id);
            Assert.AreEqual(model.Name, dto.Name);
            Assert.AreEqual(model.Description, dto.Description);
        }

        [TestMethod()]
        public void ToDoTest_WhenDtoIsNullReturnsNull()
        {
            ProductOption model = converter.ToDO(null);
            Assert.IsNull(model);
        }

        [TestMethod()]
        public void ToDOTest()
        {
            ProductOptionDto dto = new ProductOptionDto
            {
                Id = Guid.NewGuid(),
                Name = "Dummy",
                Description = ""
            };
            ProductOption model = converter.ToDO(dto);
            Assert.IsNotNull(model);
            Assert.AreEqual(dto.Id, model.Id);
            Assert.AreEqual(dto.Name, model.Name);
            Assert.AreEqual(dto.Description, model.Description);
        }
    }
}