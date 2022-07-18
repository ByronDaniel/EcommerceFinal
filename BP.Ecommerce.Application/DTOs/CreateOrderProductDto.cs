using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Ecommerce.Application.DTOs
{
    public class CreateOrderProductDto
    {
        public Guid ProductId { get; set; }
        public int ProductQuantity { get; set; }
        public Guid? OrderId { get; set; }
    }
}
