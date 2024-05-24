using AutoMapper;
using SE171223.ProductManagement.Repo.Entities;
using SE171223.ProductManagement.Repo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE171223.ProductManagement.Repo.Helper
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<Product, ProductModel>();
			CreateMap<ProductModel, Product>()
				.ForMember(d => d.ProductId, opt => opt.Ignore())
				.ForMember(dest => dest.Category, opt => opt.Ignore());

			CreateMap<Category, CategoryModel>();
		}
	}
}
