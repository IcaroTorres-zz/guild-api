using System;
using System.Net;

namespace Business.Validators
{
	public static class CommonValidationMessages
	{
		public static readonly string ConflictCodeString = ((int) HttpStatusCode.Conflict).ToString();

		public static string ForConflictWithKey(string name, object key)
		{
			return $"{name} record with given key '{key}' already exists.";
		}

		public static string ForRecordNotFound(string name, Guid key)
		{
			return $"{name} record with given key '{key}' not found.";
		}
	}
}