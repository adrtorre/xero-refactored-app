using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeroRefactoredApp.Models;

namespace XeroRefactoredApp.DTOs
{
    public class ProductDoDtoConverter : IDoDtoConverter<Product, ProductDto>
    {
        public ProductDto FromDO(Product model)
        {
            if (model == null)
            {
                return null;
            }
            ProductDto dto = new ProductDto();
            dto.Id = model.Id;
            dto.Name = model.Name;
            dto.Description = model.Description;
            dto.Price = model.Price;
            dto.DeliveryPrice = model.DeliveryPrice;
            return dto;
        }

        public Product ToDO(ProductDto dto)
        {
            if (dto == null)
            {
                return null;
            }
            Product model = new Product();
            model.Id = dto.Id;
            model.Name = dto.Name;
            model.Description = dto.Description;
            model.Price = dto.Price;
            model.DeliveryPrice = dto.DeliveryPrice;
            return model;
        }
    }
}
