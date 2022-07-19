using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Ecommerce.Domain.Entities
{
    public class OrderProduct : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        [Required]
        public int ProductQuantity { get; set; } = 1;

        public decimal Total { get; set; }
    }
}
