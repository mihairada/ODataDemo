using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODataDemo.Models
{
	public class Order
	{
		public int OrderId { get; set; }

		public string Customer { get; set; }

	}
}
