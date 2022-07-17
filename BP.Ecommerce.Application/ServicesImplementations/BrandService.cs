using AutoMapper;
using BP.Ecommerce.Application.DTOs;
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
    public class BrandService : IBrandService
    {
        private readonly IGenericRepository<Brand> repository;
        private readonly IMapper mapper;
        private readonly IValidator<CreateBrandDto> validator;

        public BrandService(IGenericRepository<Brand> repository, IMapper mapper, IValidator<CreateBrandDto> validator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task<List<BrandDto>> GetAllAsync()
        {
            List<Brand> brands = await repository.GetAllAsync();
            return mapper.Map<List<BrandDto>>(brands);
        }

        public async Task<BrandDto> GetByIdAsync(Guid id)
        {
            Brand brand = await repository.GetByIdAsync(id);
            return mapper.Map<BrandDto>(brand);
        }

        public async Task<BrandDto> PostAsync(CreateBrandDto createBrandDto)
        {
            await validator.ValidateAndThrowAsync(createBrandDto);

            Brand brand = mapper.Map<Brand>(createBrandDto);
            Brand brandResult = await repository.PostAsync(brand);
            return mapper.Map<BrandDto>(brandResult);
        }

        public async Task<BrandDto> PutAsync(BrandDto brandDto)
        {
            Brand brand = mapper.Map<Brand>(brandDto);
            Brand brandResult = await repository.PutAsync(brand);
            return mapper.Map<BrandDto>(brandResult);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await repository.DeleteAsync(id);
        }
    }
}
