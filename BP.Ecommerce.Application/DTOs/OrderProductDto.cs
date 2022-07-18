using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Ecommerce.Domain.Entities
{
    public class OrderProductDto
    {
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        public int ProductQuantity { get; set; }
    }
}
