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
        [Display(Name = "Kayıt No")]
        public int Id { get; set; }

        [Display(Name = "Kayıt Tarihi")]
        public DateTime RegisterDate { get; set; } = DateTime.Now;

        [Display(Name = "Kayıt Tipi")]
        public byte Type { get; set; }

        [Display(Name = "Ürün")]
        [ForeignKey("Product")]
		public int ProductId { get; set; }

        [Display(Name = "Sayı")]
        public long Count { get; set; }

		[MaxLength(1000)]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }

		[ForeignKey("User")]
        [Display(Name = "Kaydı Giren Kullanıcı")]
        public int UserId { get; set; }
	}
}
