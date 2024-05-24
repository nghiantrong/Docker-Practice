using Microsoft.EntityFrameworkCore;
using SE171223.ProductManagement.Repo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE171223.ProductManagement.Repo.Repository
{

	public class UnitOfWork : IDisposable
	{
		private readonly MyDbContext context;
		private GenericRepository<Category> categoryRepo;
		private GenericRepository<Product> productRepo;

		public UnitOfWork(MyDbContext context)
		{
			this.context = context;
		}

		public MyDbContext Context => context;

		public GenericRepository<Category> CategoryRepo
		{
			get
			{
				if (this.categoryRepo == null)
				{
					this.categoryRepo = new GenericRepository<Category>(context);
				}
				return categoryRepo;
			}
		}
		public GenericRepository<Product> ProductRepo
		{
			get
			{
				if (this.productRepo == null)
				{
					this.productRepo = new GenericRepository<Product>(context);
				}
				return productRepo;
			}
		}

		public void Save()
		{
			context.SaveChanges();
		}

		private bool disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					context.Dispose();
				}
			}
			this.disposed = true;
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
