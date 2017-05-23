using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ODataDemo.Models
{
	public class Product
	{ 
		public int ProductId { get; set; }

		public string Name { get; set; }

		//[ForeignKey("ProductCategory")]
		public int ProductCategoryId { get; set; }

		//public virtual Category ProductCategory { get; set; }
		
	}
}