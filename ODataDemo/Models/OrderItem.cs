using System.ComponentModel.DataAnnotations.Schema;

namespace ODataDemo.Models
{
	public class OrderItem
	{
		public int OrderItemId { get; set; }

		//[ForeignKey("Order")]
		public int OrderId { get; set; }
		public virtual Order Order { get; set; }

		public int Quantity { get; set; }

		//[ForeignKey("Product")]
		public int ProductId { get; set; }
		public virtual Product Product { get; set; }
	}
}