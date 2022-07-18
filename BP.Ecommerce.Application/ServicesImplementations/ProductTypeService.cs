using AutoMapper;
using BP.Ecommerce.Application.DTOs;
using BP.Ecommerce.Application.Exceptions;
using BP.Ecommerce.Application.ServicesInterfaces;
using BP.Ecommerce.Domain.Entities;
using BP.Ecommerce.Domain.RepositoriesInterfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Ecommerce.Application.ServicesImplementations
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly IGenericRepository<ProductType> repository;
        private readonly IMapper mapper;
        private readonly IValidator<CreateProductTypeDto> validator;

        public ProductTypeService(IGenericRepository<ProductType> repository, IMapper mapper, IValidator<CreateProductTypeDto> validator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task<List<ProductTypeDto>> GetAllAsync(string? search, string? sort, string? order, int? limit = 0, int? offset = 0)
        {
            List<ProductType> brands = await repository.GetAllAsync(search, sort, order, limit, offset);
            return mapper.Map<List<ProductTypeDto>>(brands);
        }

        public async Task<ProductTypeDto> GetByIdAsync(Guid id)
        {
            ProductType productType = await repository.GetByIdAsync(id);
            if (productType == null)
                throw new NotFoundException($"No existe el registro con id: {id}");

            return mapper.Map<ProductTypeDto>(productType);
        }

        public async Task<ProductTypeDto> PostAsync(CreateProductTypeDto createBrandDto)
        {
            await validator.ValidateAndThrowAsync(createBrandDto);

            ProductType productType = mapper.Map<ProductType>(createBrandDto);
            ProductType brandResult = await repository.PostAsync(productType);
            return mapper.Map<ProductTypeDto>(brandResult);
        }

        public async Task<ProductTypeDto> PutAsync(ProductTypeDto brandDto)
        {
            ProductType productType = mapper.Map<ProductType>(brandDto);
            ProductType brandResult = await repository.PutAsync(productType);
            return mapper.Map<ProductTypeDto>(brandResult);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await repository.DeleteAsync(id);
        }
    }
}
