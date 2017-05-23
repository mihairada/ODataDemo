using ODataDemo.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace ODataDemo.Controllers
{
    public class ProductsController : ODataController
    {
		//private List<Product> products = new List<Product>
		//{
		//	new Product()
		//	{
		//		ID = 1,
		//		Name = "Test Product 1"
		//	},
		//	new Product()
		//	{
		//		ID = 2,
		//		Name = "Test Product 2"
		//	},
		//	new Product()
		//	{
		//		ID = 3,
		//		Name = "Test Product 3"
		//	},
		//	new Product()
		//	{
		//		ID = 4,
		//		Name = "Test Product 4"
		//	}

		//};

		ProductServiceContext db = new ProductServiceContext();

		// Needed with the WebAPIConfig setting config.Count().Filter().OrderBy().Expand().Select().MaxTop(null); - to allow URL filters
		[EnableQuery]
		public IQueryable<Product> Get()
		{
			return db.Products;
		}

		// By Convention Same as GetProduct - "key" parameter is also by convention - needs to be decorated with [FromODataURI] for convention routing to work
		// Return type can be IQueryable<T> or T
		// https://docs.microsoft.com/en-us/aspnet/web-api/overview/odata-support-in-aspnet-web-api/odata-routing-conventions
		public IQueryable<Product> Get([FromODataUri] int key)
		{
			return db.Products.Where(p => p.ProductId == key).Select(pr => pr).AsQueryable();
		}

		// Create new entity
		public async Task<IHttpActionResult> Post(Product product)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			db.Products.Add(product);
			await db.SaveChangesAsync();
			return Created(product);
		}

		// Update entire entity
		public async Task<IHttpActionResult> Put([FromODataUri]int key, Product product)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (key != product.ProductId)
			{
				return BadRequest("Updating the worng product");
			}

			db.Entry(product).State = EntityState.Modified;

			try
			{
				await db.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ProductExists(key))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}
			return Updated(product);
		}


		// Update properties on the entity
		public async Task<IHttpActionResult> Patch([FromODataUri]int key, Delta<Product> product)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var entity = await db.Products.FindAsync(key);

			if (entity == null)
			{
				return NotFound();
			}

			product.Patch(entity);

			try
			{
				await db.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ProductExists(key))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}
			return Updated(product);
		}

		// Delete the entity
		public async Task<IHttpActionResult> Delete([FromODataUri] int key)
		{
			var product = await db.Products.FindAsync(key);
			if (product == null)
			{
				return NotFound();
			}
			db.Products.Remove(product);
			await db.SaveChangesAsync();
			return StatusCode(HttpStatusCode.NoContent);
		}

		// In a typical IIS configuration, the dot in this URL will cause IIS to return error 404. You can resolve this by adding the following section to your Web.Config file:
		//https://docs.microsoft.com/en-us/aspnet/web-api/overview/odata-support-in-aspnet-web-api/odata-v4/odata-actions-and-functions

		// Bound Action 
		[HttpPost]
		public async Task<IHttpActionResult> ChangeCategory([FromODataUri] int key, [FromBody]ODataActionParameters parameters)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest();
				}
				var product = await db.Products.FindAsync(key);
				if (product == null)
				{
					return NotFound();
				}
				product.ProductCategoryId = (int)parameters["CategoryId"];
			
				await db.SaveChangesAsync();
				return Ok();
			}
			catch (DbUpdateException e)
			{
				if (!ProductExists(key))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}
		}

		// Bound Function 
		[HttpGet]
		//public async Task<IHttpActionResult> MostPopular([FromODataUri] int categoryId)
		public string MostPopular([FromODataUri] int categoryId)
		//public int MostPopular()
		{
			var mostPopular = (from product in db.Products
							   join oi in db.OrderItems on product.ProductId equals oi.ProductId
							   where product.ProductCategoryId == categoryId
							   group oi by oi.ProductId into g
							   select new { ProductId = g.Key, CountProduct = g.Count() })
							   .OrderByDescending(d => d.CountProduct);
			if (mostPopular.Any())
			{
				var mostPopularProductId = (mostPopular.FirstOrDefault()).ProductId;
				//var mostPopularProduct = db.Products.Find(mostPopularProductId);
				var mostPopularProduct = db.Products.Where(p => p.ProductId == mostPopularProductId).FirstOrDefault();
				//return Ok(mostPopularProductId);
				return mostPopularProduct.Name;
			}
			//return NotFound();
			return string.Empty;
		}

		[HttpGet]
		[ODataRoute("ProductCount()")]
		public IHttpActionResult ProductCount()
		{
			return Ok(db.Products.Count());
		}


		private bool ProductExists(int key)
		{
			return db.Products.Any(p => p.ProductId == key);
		}

		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}
	}
}
