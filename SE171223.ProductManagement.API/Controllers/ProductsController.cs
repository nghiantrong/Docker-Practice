using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE171223.ProductManagement.Repo.Entities;
using SE171223.ProductManagement.Repo.Models;
using SE171223.ProductManagement.Repo.Repository;
using Swashbuckle.AspNetCore.Annotations;

namespace SE171223.ProductManagement.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly UnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ProductsController(UnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}


		// GET: api/Products
		[SwaggerOperation(Description = "SortBy (ProductId = 1,ProductName = 2,CategoryId = 3,UnitsInStock = 4,UnitPrice = 5,) <br>" +
			"SortType (Ascending = 1,Descending = 2,)")]
		[HttpGet]
		public IActionResult GetProducts([FromQuery] RequestSearchProductModel requestSearchProduct)
		{
			if (_unitOfWork.ProductRepo == null)
			{
				return NotFound();
			}

			var sortBy = requestSearchProduct.Sort != null ? requestSearchProduct.Sort?.sortBy.ToString() : null;
			var sortType = requestSearchProduct.Sort != null ? requestSearchProduct.Sort?.sortType.ToString() : null;

			Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null;
			if (!string.IsNullOrEmpty(sortBy))
			{
				if (sortType == SortType.Ascending.ToString())
				{
					orderBy = query => query.OrderBy(p => EF.Property<object>(p, sortBy));
				}
				else if (sortType == SortType.Descending.ToString())
				{
					orderBy = query => query.OrderByDescending(p => EF.Property<object>(p, sortBy));
				}
			}

			var products = _unitOfWork.ProductRepo.Get(includeProperties: "Category", orderBy: orderBy,
				pageIndex: requestSearchProduct.pageIndex, pageSize: requestSearchProduct.pageSize);

			if (requestSearchProduct.minPrice.HasValue)
			{
				products = products.Where(p => p.UnitPrice >=  requestSearchProduct.minPrice.Value);
			}
			if (requestSearchProduct.maxPrice.HasValue)
			{
				products = products.Where(p => p.UnitPrice <=  requestSearchProduct.maxPrice.Value);
			}

			// Map products to ProductModel
			var productModels = _mapper.Map<List<ProductModel>>(products);

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			return Ok(productModels);
		}

		// GET: api/Products/5
		[HttpGet("{id}")]
		public ActionResult<Product> GetProduct(int id)
		{
			if (_unitOfWork.ProductRepo == null)
			{
				return NotFound();
			}
			var product = _unitOfWork.ProductRepo.GetByID(id);
			product.Category = _unitOfWork.CategoryRepo.Get().FirstOrDefault(c => c.CategoryId == product.CategoryId);
			
			if (product == null)
			{
				return NotFound();
			}

			return product;
		}

		//// PUT: api/Products/5
		[HttpPut("{id}")]
		public IActionResult PutProduct(int id, ProductModel productModel)
		{
			if (productModel == null)
			{
				return BadRequest(ModelState);
			}
			if (id != productModel.ProductId)
			{
				return BadRequest(ModelState);
			}
			if (!_unitOfWork.ProductRepo.Get().Any(p => p.ProductId == id))
			{
				return NotFound();
			}
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var existingProduct = _unitOfWork.ProductRepo.Get().FirstOrDefault(p => p.ProductId == id);
			if (existingProduct == null)
			{
				return NotFound();
			}

			var productMap = _mapper.Map<Product>(productModel);
			productMap.ProductId = id;
			_unitOfWork.Context.Entry(existingProduct).State = EntityState.Detached;
			_unitOfWork.ProductRepo.Update(productMap);
			_unitOfWork.Save();


			return NoContent();
		}

		// POST: api/Products
		[HttpPost]
		public ActionResult<Product> PostProduct(ProductModel product)
		{
			if (_unitOfWork.ProductRepo == null)
			{
				return Problem("Entity set 'MyDbContext.Products'  is null.");
			}
			var productMap = _mapper.Map<Product>(product);

			_unitOfWork.ProductRepo.Insert(productMap);
			_unitOfWork.Save();

			return CreatedAtAction("GetProduct", new { id = productMap.ProductId }, productMap);
		}

		// DELETE: api/Products/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			if (_unitOfWork.ProductRepo == null)
			{
				return NotFound();
			}
			var product = _unitOfWork.ProductRepo.GetByID(id);
			if (product == null)
			{
				return NotFound();
			}

			_unitOfWork.ProductRepo.Delete(product);
			_unitOfWork.Save();

			return NoContent();
		}

		//private bool ProductExists(int id)
		//{
		//    return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
		//}
	}
}
