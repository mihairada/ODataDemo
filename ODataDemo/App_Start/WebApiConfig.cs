using ODataDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace ODataDemo
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			var builder = new ODataConventionModelBuilder();

			//http://stackoverflow.com/questions/39515218/odata-error-the-query-specified-in-the-uri-is-not-valid-the-property-cannot-be
			config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);

			//builder.EntitySet<Product>("Products").EntityType.Ignore( p => p.ProductCategory);		// needed for serialization to work
			builder.EntitySet<Product>("Products");

			builder.EntitySet<Category>("Categories");

			builder.EntitySet<Order>("Orders");

			builder.EntitySet<OrderItem>("OrderItems");

			builder.Namespace = "ProductService";
			builder.EntityType<Product>()
				.Action("ChangeCategory")
				.Parameter<int>("CategoryId");

			builder.EntityType<Product>().Collection
				.Function("MostPopular")
				.Returns<int>()
				.Parameter<int>("CategoryId");

			builder.Function("ProductCount")
				.Returns<int>();

			config.MapODataServiceRoute("ODataRoute", "ODataAPI", builder.GetEdmModel());
		}
	}
}
