using System.ComponentModel.DataAnnotations;

namespace DBTriggerTest.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
