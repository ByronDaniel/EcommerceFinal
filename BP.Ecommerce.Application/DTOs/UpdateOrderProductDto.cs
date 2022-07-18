using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Ecommerce.Application.DTOs
{
    public class UpdateOrderProductDto
    {
        public Guid ProductId { get; set; }
        public int ProductQuantity { get; set; }
    }
}
