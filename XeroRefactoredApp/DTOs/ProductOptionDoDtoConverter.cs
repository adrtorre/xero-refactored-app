using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeroRefactoredApp.Models;

namespace XeroRefactoredApp.DTOs
{
    public class ProductOptionDoDtoConverter : IDoDtoConverter<ProductOption, ProductOptionDto>
    {
        public ProductOptionDto FromDO(ProductOption model)
        {
            if (model == null)
            {
                return null;
            }
            ProductOptionDto dto = new ProductOptionDto();
            dto.Id = model.Id;
            dto.Name = model.Name;
            dto.Description = model.Description;
            dto.ProductId = model.ProductId;
            return dto;
        }

        public ProductOption ToDO(ProductOptionDto dto)
        {
            if (dto == null)
            {
                return null;
            }
            ProductOption model = new ProductOption();
            model.Id = dto.Id;
            model.Name = dto.Name;
            model.Description = dto.Description;
            model.ProductId = dto.ProductId;
            return model;
        }
    }
}
