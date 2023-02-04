using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Domain.ValueObjects
{
	public class Result
	{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		private Result() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		[JsonIgnore]
		public ushort Rv { get; set; }
		public bool IsSuccess { get => Rv == 0; }
		public string Message { get; set; }
		public string[] Parameters { get; set; }
		public static Result Success(string msg = "Success") => new() { Rv = 0, Message = msg };
		public static Result Error(ushort rv, string msg) => new() { Rv = rv, Message = msg };
	}
}
