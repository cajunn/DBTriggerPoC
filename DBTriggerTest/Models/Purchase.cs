using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DBTriggerTest.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        [ValidateNever]
        public virtual Product Product { get; set; } 
        public int CustomerId { get; set; }
        [ValidateNever]
        public virtual Customer Customer { get; set; } 

    }
}
