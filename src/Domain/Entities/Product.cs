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
	public class Product : IEfEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ProductNo { get; set; }

		[MaxLength(128)]
		public string Name { get; set; }

		[MaxLength(512)]
		public string Description { get; set; }

		public long Stock { get; set; }

		public DateTime RegisterDate { get; set; } = DateTime.Now;
		public DateTime? DeletedDate { get; set; }
	}
}
