﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BP.Ecommerce.Domain.Entities
{
    public class Order : AuditoryEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? DeliveryMethodId { get; set; }
        [ForeignKey("DeliveryMethodId")]
        public DeliveryMethod? DeliveryMethod { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TotalPrice { get; set; }
        public virtual List<OrderProduct> orderProducts { get; set; }
    }
}
