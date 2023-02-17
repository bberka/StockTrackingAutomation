namespace Domain.Entities
{
    public class Supplier : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Tedarikçi No")]
        public int Id { get; set; }

        [Display(Name = "Kayıt Tarihi")]
        public DateTime RegisterDate { get; set; } = DateTime.Now;

        [MaxLength(128)]
        [Display(Name = "Ad Soyad")]
        public string Name { get; set; }

        [MaxLength(128)]
        [Display(Name = "Şirket Adı")]
        public string CompanyName { get; set; }

        [MaxLength(128)]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [MaxLength(32)]
        [Display(Name = "Tel No")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Borç")]
        public double Debt { get; set; } = 0;

        [Display(Name = "Silinme Tarihi")]
        public DateTime? DeletedDate { get; set; }

        public virtual List<DebtLog> DebtLogs { get; set; } = new();
        public virtual List<Purchase> Purchases { get; set; } = new();
    }
}
