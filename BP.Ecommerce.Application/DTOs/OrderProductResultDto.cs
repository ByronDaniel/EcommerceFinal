namespace BP.Ecommerce.Domain.Entities
{
    public class OrderProductResultDto
    {
        public string Product { get; set; }
        public decimal Price { get; set; }
        public int ProductQuantity { get; set; }
        public decimal Total { get; set; }
    }
}
