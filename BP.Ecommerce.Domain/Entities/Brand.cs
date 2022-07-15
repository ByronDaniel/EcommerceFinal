using System.ComponentModel.DataAnnotations;

namespace BP.Ecommerce.Domain.Entities
{
    public class Brand
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(30)]
        public string Name { get; set; }
    }
}
