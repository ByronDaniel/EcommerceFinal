﻿namespace BP.Ecommerce.Application.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public Guid ProductTypeId { get; set; }

        public Guid BrandId { get; set; }

        public string? Brand { get; set; }
    }
}
