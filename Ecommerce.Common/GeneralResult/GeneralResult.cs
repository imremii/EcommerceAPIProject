using System.Text.Json.Serialization;
namespace Ecommerce.Common
{
        public class GeneralResult
        {
            public bool IsSuccess { get; set; }
            public string Message { get; set; } = string.Empty;

            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public Dictionary<string, List<Error>>? Errors { get; set; }
            public static GeneralResult SuccessResult(string message = "Success")
                => new() { IsSuccess = true, Message = message, Errors = null };

            public static GeneralResult NotFound(string message = "Resource not found.")
                => new() { IsSuccess = false, Message = message, Errors = null };

            public static GeneralResult FailResult(string message = "Operation faild.")
                => new() { IsSuccess = false, Message = message, Errors = null };

            public static GeneralResult FailResult(Dictionary<string, List<Error>> errors, string message = "One or more validation errors occurred.")
                => new() { IsSuccess = false, Message = message, Errors = errors };
        }

        public class GeneralResult<T> : GeneralResult
        {
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
            public T? Data { get; set; }

            public static GeneralResult<T> SuccessResult(T data, string message = "Success")
                => new() { IsSuccess = true, Message = message, Data = data, Errors = null };

            public static new GeneralResult<T> SuccessResult(string message = "Success")
                 => new() { IsSuccess = true, Message = message, Errors = null };

            public static new GeneralResult<T> NotFound(string message = "Resource not found")
                => new() { IsSuccess = false, Message = message, Errors = null };

            public static new GeneralResult<T> FailResult(string message = "Operation faild.")
                => new() { IsSuccess = false, Message = message, Errors = null };

            public static new GeneralResult<T> FailResult(Dictionary<string, List<Error>> errors, string message = "One or more validation errors occurred.")
                => new() { IsSuccess = false, Message = message, Errors = errors };
        }
}
