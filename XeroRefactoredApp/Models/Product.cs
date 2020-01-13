using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace XeroRefactoredApp.Models
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name
        {
            get; set;
        }
        [StringLength(500)]
        public string Description
        {
            get; set;
        }
        [Required(ErrorMessage = "Price is required")]
        [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Invalid Price; Maximum Two Decimal Points.")]
        [Range(0, 9999999999999999.99, ErrorMessage = "Invalid Price; Max 18 digits")]
        public decimal Price
        {
            get; set;
        }
        [Required(ErrorMessage = "Delivery Price is required")]
        [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Invalid Delivery Price; Maximum Two Decimal Points.")]
        [Range(0, 9999999999999999.99, ErrorMessage = "Invalid Delivery Price; Max 18 digits")]
        public decimal DeliveryPrice
        {
            get; set;
        }

        public override bool Equals(object obj)
        {
            return obj is Product product &&
                   Id.Equals(product.Id) &&
                   Name == product.Name &&
                   Description == product.Description &&
                   Price == product.Price &&
                   DeliveryPrice == product.DeliveryPrice;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Description, Price, DeliveryPrice);
        }
    }
}
