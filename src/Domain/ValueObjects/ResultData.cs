using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Domain.ValueObjects
{
	public class ResultData<T>
	{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		private ResultData() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		[JsonIgnore]
		public ushort Rv { get; set; }
		public bool IsSuccess { get => Rv == 0; }
		public string Message { get; set; }
		public T? Data { get; set; }
		public static ResultData<T> Success(T data) => new() { Rv = 0, Message = "Success", Data = data };
		public static ResultData<T> Success(T data,string msg) => new() { Rv = 0, Message = msg, Data = data };
		public static ResultData<T> Error(ushort rv, string msg) => new() { Rv = rv, Message = msg };
	}
}
