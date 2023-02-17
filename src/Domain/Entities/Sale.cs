

namespace Domain.Entities
{
    public class Sale : IEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Kayıt No")]
        public int Id { get; set; }

        [Display(Name = "Kayıt Tarihi")]
        public DateTime RegisterDate { get; set; } = DateTime.Now;

        [Display(Name = "Ürün")]
        [ForeignKey("Product")]
		public int ProductId { get; set; }

        [Display(Name = "Ürün Sayısı")]
        public long Count { get; set; }

        [Display(Name = "Müşteri")]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [Display(Name = "Birim Fiyat")]
        public double PricePerUnit  { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

		[ForeignKey("User")]
        [Display(Name = "Kaydı Giren Kullanıcı")]
        public int UserId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}
