

namespace Domain.Entities
{
 
    public class DebtLog : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Kayıt No")]
        public int Id { get; set; }

        [Display(Name = "Kayıt Tarihi")]
        public DateTime RegisterDate { get; set; } = DateTime.Now;

        [Display(Name = "Kayıt Tipi")]
        public byte Type { get; set; }

        [Display(Name = "Kayıt Tipi")]
        public string TypeString
        {
            get
            {
                return Type switch
                {
                    1 => "Alınan",
                    2 => "Verilen",
                    _ => "Tanımlı Değil",
                };
            }
        }
        [ForeignKey("Customer")]
        [Display(Name = "Müşteri")]
        public int? CustomerId { get; set; }

        [ForeignKey("Supplier")]
        [Display(Name = "Tedarikçi")]
        public int? SupplierId { get; set; }

        [Display(Name = "Para Miktarı")]
        public double Money { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [ForeignKey("User")]
        [Display(Name = "Kaydı Giren Kullanıcı")]
        public int UserId { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public virtual User User { get; set; }
    }
}
