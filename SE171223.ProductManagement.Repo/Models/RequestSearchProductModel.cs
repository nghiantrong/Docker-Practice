using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SE171223.ProductManagement.Repo.Models
{
	public class RequestSearchProductModel
	{
		[SwaggerIgnore]
		public string? ProductName { get; set; }
		[SwaggerIgnore]
		public int? CategoryId { get; set; }
		public decimal? minPrice { get; set; } = decimal.Zero;
		public decimal? maxPrice { get; set; } = null;
		public Sort? Sort { get; set; }
		public int pageIndex { get; set; } = 1;
		public int pageSize { get; set; } = 5;
	}

	public class Sort
	{
		public SortProductBy sortBy { get; set; }
		public SortType sortType { get; set; }
	}

    public enum SortProductBy
    {
		ProductId = 1,
		ProductName = 2,
		CategoryId = 3,
		UnitsInStock = 4,
		UnitPrice = 5
	}

	public enum SortType
	{
		Ascending = 1,
		Descending = 2
	}
}
