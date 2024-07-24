using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using EMS.ResponseModel.Enums;

namespace EMS.ResponseModel;
public class ApiResponse<T>
{
    [JsonConverter(typeof(StringEnumConverter))]
    public ResponseStatus Status { get; set; }
    public T Data { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public ErrorCode? ErrorCode { get; set; }

    public ApiResponse(ResponseStatus status, T data, ErrorCode? errorCode = null)
    {
        Status = status;
        Data = data;
        ErrorCode = errorCode;
    }
}

