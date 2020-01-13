using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XeroRefactoredApp.DTOs
{
    public class ProductDto
    {

        public Guid Id { get; set; }
        public string Name
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }
        public decimal Price
        {
            get; set;
        }
        public decimal DeliveryPrice
        {
            get; set;
        }

        public override bool Equals(object obj)
        {
            return obj is ProductDto dto &&
                   Id.Equals(dto.Id) &&
                   Name == dto.Name &&
                   Description == dto.Description &&
                   Price == dto.Price &&
                   DeliveryPrice == dto.DeliveryPrice;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Description, Price, DeliveryPrice);
        }
    }
}
