using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
	public class LoginModel
	{
		[Display(Name = "Email Address")]
		public string EmailAddress { get; set; }

		[Display(Name = "Şifre")]
		public string Password { get; set; }
	}
}
