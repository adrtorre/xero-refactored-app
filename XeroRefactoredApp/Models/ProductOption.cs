using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace XeroRefactoredApp.Models
{
    public class ProductOption
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id
        {
            get; set;
        }
        [Required]
        public Guid ProductId
        {
            get; set;
        }
        [Required]
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
            return obj is ProductOption option &&
                   Id.Equals(option.Id) &&
                   ProductId.Equals(option.ProductId) &&
                   Name == option.Name &&
                   Description == option.Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, ProductId, Name, Description);
        }
    }
}
