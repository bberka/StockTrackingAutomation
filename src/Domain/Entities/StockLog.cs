using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasMe.EFCore;

namespace Domain.Entities
{
	public class StockLog : IEfEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public DateTime RegisterDate { get; set; } = DateTime.Now;

		public byte Type { get; set; }

		[ForeignKey("Product")]
		public int ProductId { get; set; }

		public long Count { get; set; }

		[MaxLength(1000)]
		public string Description { get; set; }

		[ForeignKey("User")]
		public int UserId { get; set; }
	}
}
