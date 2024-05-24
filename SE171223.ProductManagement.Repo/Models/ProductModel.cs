using SE171223.ProductManagement.Repo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE171223.ProductManagement.Repo.Models
{
	public class ProductModel
	{
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public int CategoryId { get; set; }
		public int UnitsInStock { get; set; }
		public decimal UnitPrice { get; set; }
	}
}
