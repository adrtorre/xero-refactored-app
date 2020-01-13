using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XeroRefactoredApp.DTOs
{
    public class ProductOptionDto
    {
        public Guid Id
        {
            get; set;
        }
        public Guid ProductId
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }

        public override bool Equals(object obj)
        {
            return obj is ProductOptionDto dto &&
                   Id.Equals(dto.Id) &&
                   ProductId.Equals(dto.ProductId) &&
                   Name == dto.Name &&
                   Description == dto.Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, ProductId, Name, Description);
        }
    }
}
