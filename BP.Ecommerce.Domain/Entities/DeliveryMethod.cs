using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Ecommerce.Domain.Entities
{
    public class DeliveryMethod : CatalogueEntity
    {
        [Required]
        public string Description { get; set; }
    }
}
