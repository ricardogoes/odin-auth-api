using System.Text.Json.Serialization;

namespace Odin.Auth.Domain.Models
{
    public class ApiResponse
    {
        public string State { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object Data { get; set; }


        public ApiResponse(string state, string message, object data)
        {
            State = state;
            Message = message;
            Data = data;
        }

        public ApiResponse(string state, string message)
        {
            State = state;
            Message = message;
        }

        public ApiResponse(string state, object data)
        {
            State = state;
            Data = data;
        }
    }

    public class ApiResponseState
    {
        public const string Failed = "FAILED";
        public const string NotAuth = "NOT_AUTH";
        public const string Success = "SUCCESS";
    }

}
