using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Ecommerce.Domain.Entities
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid? DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public decimal? Subtotal { get; set; } = 0;
        public decimal? TotalPrice { get; set; } = 0;
        public string State { get; set; }
        public virtual List<OrderProductDto> orderProducts { get; set; }
    }
}
