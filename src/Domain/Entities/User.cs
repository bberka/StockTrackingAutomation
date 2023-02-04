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
	public class User : IEfEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int UserNo { get; set; }

		public bool IsValid { get; set; } = true;

		public DateTime RegisterDate { get; set; } = DateTime.Now;

		[MaxLength(128)]
		public string EmailAddress { get; set; }

		[MaxLength(128)]
		public string Password { get; set; }


		[MaxLength(64)]
		public string? LastLoginIp { get; set; }

		[MaxLength(500)]
		public string? LastLoginUserAgent { get; set; }

		public DateTime? LastLoginDate { get; set; }

		public int FailedPasswordCount { get; set; } = 0;

		public DateTime? PasswordLastUpdateDate { get; set; }
		public DateTime? DeletedDate { get; set; }

		public int RoleType { get; set; }

	}
}
