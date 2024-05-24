using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SE171223.ProductManagement.Repo.Entities
{
	public class MyDbContext : DbContext
	{
		public MyDbContext() { }
		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	var builder = new ConfigurationBuilder()
		//		.SetBasePath(Directory.GetCurrentDirectory())
		//		.AddJsonFile("D:\\FPT_UNIVERSITY\\HK8\\PRN231\\Lab1\\SE171223.ProductManagement.API\\SE171223.ProductManagement.API\\appsettings.json", optional: true, reloadOnChange: true);
		//	IConfigurationRoot configuration = builder.Build();
		//	optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyDbContext"));
		//}

		public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
		{
			try
			{
				var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
				if (databaseCreator != null)
				{
					if (!databaseCreator.CanConnect()) databaseCreator.Create();
					if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		public virtual DbSet<Product> Products { get; set; }
		public virtual DbSet<Category> Categories { get; set; }

		protected override void OnModelCreating(ModelBuilder optionsBuilder)
		{
			base.OnModelCreating(optionsBuilder);
			optionsBuilder.Entity<Category>().HasData(
				new Category { CategoryId = 1, CategoryName = "Beverages"},
				new Category { CategoryId = 2, CategoryName = "Condiments"},
				new Category { CategoryId = 3, CategoryName = "Confections"},
				new Category { CategoryId = 4, CategoryName = "Dairy Products"},
				new Category { CategoryId = 5, CategoryName = "Grains/Cereals"},
				new Category { CategoryId = 6, CategoryName = "Meat/Poultry"},
				new Category { CategoryId = 7, CategoryName = "Produce"},
				new Category { CategoryId = 8, CategoryName = "Seafood"}
			);
			optionsBuilder.Entity<Product>().HasData(
				new Product
				{
					ProductId = 1,
					ProductName = "Hello",
					CategoryId = 1,
					UnitsInStock = 1,
					UnitPrice = 5,
				},
				new Product
				{
					ProductId = 2,
					ProductName = "Goodbye",
					CategoryId = 2,
					UnitsInStock = 2,
					UnitPrice = 6,
				},
				new Product
				{
					ProductId = 3,
					ProductName = "Beautiful",
					CategoryId = 3,
					UnitsInStock = 3,
					UnitPrice = 7,
				},
				new Product
				{
					ProductId = 5,
					ProductName = "BigBoy",
					CategoryId = 5,
					UnitsInStock = 5,
					UnitPrice = 8,
				},
				new Product
				{
					ProductId = 6,
					ProductName = "SmallBoy",
					CategoryId = 6,
					UnitsInStock = 6,
					UnitPrice = 8,
				},
				new Product
				{
					ProductId = 7,
					ProductName = "Pretty",
					CategoryId = 7,
					UnitsInStock = 7,
					UnitPrice = 9,
				}
				);
		}
	}
}
