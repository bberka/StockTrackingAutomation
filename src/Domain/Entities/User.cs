namespace Domain.Entities
{
    public class User : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Kullanıcı No")]
        public int Id { get; set; }

        [Display(Name = "Kullanıcı Geçerliliği")]
        public bool IsValid { get; set; } = true;

        [Display(Name = "Kayıt Tarihi")]
        public DateTime RegisterDate { get; set; } = DateTime.Now;

        [MaxLength(128)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [MaxLength(128)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }


        [MaxLength(64)]
        [Display(Name = "En Son Giriş Ip")]
        public string? LastLoginIp { get; set; }

        [MaxLength(500)]
        [Display(Name = "En Son Giriş Browser")]
        public string? LastLoginUserAgent { get; set; }

        [Display(Name = "En Son Giriş Tarihi")]
        public DateTime? LastLoginDate { get; set; }

        [Display(Name = "Yanlış Şifre Sayısı")]
        public int FailedPasswordCount { get; set; } = 0;

        [Display(Name = "Şifre Güncelleme Tarihi")]
        public DateTime? PasswordLastUpdateDate { get; set; }

        [Display(Name = "Silinme Tarihi")]
        public DateTime? DeletedDate { get; set; }


        [Display(Name = "Rol")]
        public int RoleType { get; set; }

        [Display(Name = "Rol")]
        public string RoleString
        {
            get
            {
                switch (RoleType)
                {
                    case 1: return "Moderator";
                    case 2: return "Owner";
                    default: return "Geçersiz";
                }
            }
        }

    }
}
