using AutoMapper;
using DistributedSystem.Contract.Abstractions.Shared;
using DistributedSystem.Contract.Services.V1.Product;
using DistributedSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedSystem.Application.Mappers
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<Product, Response.ProductResponse>().ReverseMap();
            CreateMap<PagedResult<Product>, PagedResult<Response.ProductResponse>>().ReverseMap();
            //CreateMap<Product, Response.ProductResponse>().ReverseMap();
            //CreateMap<PagedResult<Product>, PagedResult<Response.ProductResponse>>().ReverseMap();
            //CreateMap<Product, Contract.Services.V2.Products.Response.ProductResponse>().ReverseMap();
        }
    }
}
