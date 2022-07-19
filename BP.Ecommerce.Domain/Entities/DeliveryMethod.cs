using System.ComponentModel.DataAnnotations;

namespace BP.Ecommerce.Domain.Entities
{
    public class DeliveryMethod : CatalogueEntity
    {
        [Required]
        public string Description { get; set; }
    }
}
