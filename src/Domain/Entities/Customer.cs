using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using EasMe.EFCore;

namespace Domain.Entities
{
    public class Customer : IEfEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Müşteri No")]
        public int CustomerNo { get; set; }

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
        public string PhoneNumber { get; set; }

        [Display(Name = "Borç")]
        public double Debt { get; set; } = 0;

        [Display(Name = "Silinme Tarihi")]
        public DateTime? DeletedDate { get; set; }
    }
}
