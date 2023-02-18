namespace Domain.Models;

public class ChangePasswordModel
{
    [MaxLength(32)]
    [Display(Name = "Şifre")]
    public string OldPassword { get; set; }

    [MaxLength(32)]
    [Display(Name = "Yeni Şifre")]
    public string NewPassword { get; set; }

    [MaxLength(32)]
    [Display(Name = "Yeni Şifre Doğrula")]
    public string NewPasswordConfirm { get; set; }
}