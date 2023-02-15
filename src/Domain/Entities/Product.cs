

namespace Domain.Entities
{
    public class Product : IEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Display(Name = "Ürün No")]
        public int Id { get; set; }

		[MaxLength(128)]
        [Display(Name = "Ürün Adı")]
        public string Name { get; set; }

		[MaxLength(512)]

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Stok")]
        public long Stock { get; set; }

        [Display(Name = "Kayıt Tarihi")]
        public DateTime RegisterDate { get; set; } = DateTime.Now;

        [Display(Name = "Silinme Tarihi")]
        public DateTime? DeletedDate { get; set; }
	}
}
