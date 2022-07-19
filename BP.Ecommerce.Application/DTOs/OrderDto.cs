namespace BP.Ecommerce.Domain.Entities
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public virtual List<OrderProductResultDto> orderProducts { get; set; }
        public Guid? DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public decimal Subtotal { get; set; } = 0;
        public decimal Iva { get; set; } = 0;
        public decimal TotalPrice { get; set; } = 0;
        public string State { get; set; }
    }
}
