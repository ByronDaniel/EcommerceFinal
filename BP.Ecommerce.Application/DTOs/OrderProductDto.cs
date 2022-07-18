﻿using System;
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
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string Product { get; set; }
        public int ProductQuantity { get; set; }
        public decimal Total { get; set; }
    }
}
