using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ODataDemo.Models
{
	public class ProductServiceContext : DbContext
	{
		public ProductServiceContext() : base ("ProductContext")
		{
		}

		public DbSet<Product> Products { get; set; }

		public DbSet<Category> Categories { get; set; }

		public DbSet<Order> Orders { get; set; }

		public DbSet<OrderItem> OrderItems { get; set; }

	}
}