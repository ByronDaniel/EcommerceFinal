using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Ecommerce.Domain.Entities
{
    public class Product : AuditoryEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public Guid ProductTypeId { get; set; }

        [ForeignKey("ProductTypeId")]
        public ProductType ProductType { get; set; }
        
        [Required]
        public Guid BrandId { get; set; }

        [ForeignKey("BrandId")]
        public Brand Brand { get; set; }
    }
}
