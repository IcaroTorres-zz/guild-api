using System;
using System.Net;

namespace Business.Validators
{
  public static class CommonValidationMessages
  {
    public static string NotFoundCodeString = ((int)HttpStatusCode.NotFound).ToString();
    public static string ConflictCodeString = ((int)HttpStatusCode.Conflict).ToString();
    public static string GoneCodeString = ((int)HttpStatusCode.Gone).ToString();
    public static string BadRequestCodeString = ((int)HttpStatusCode.BadRequest).ToString();

    public static string ForConflictWithKey(string name, object key)
    {
      return $"{name} record with given key '{key}' already exists.";
    }

    public static string ForRecordNotFound(string name, Guid key)
    {
      return $"{name} record with given key '{key}' not found.";
    }

    public static string ForInvalidRedord(string name)
    {
      return $"{name} record is invalid.";
    }

    public static string ForRecordGone(string name, Guid key)
    {
      return $"{name} record with given key '{key}' was removed or disabled.";
    }

    public static string ForBadRequest(string name)
    {
      return $"{name} record is malformed.";
    }
  }
}
