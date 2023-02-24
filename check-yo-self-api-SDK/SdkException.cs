using System;
using System.Collections.Generic;

namespace check_yo_self_api_client;

public class SdkException : Exception
{
    public int StatusCode { get; set; }
    public string ExceptionMessage { get; set; }
    public string ResponseData { get; set; }
    public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; set; }

    public SdkException() { }

    // The signature of this method is what gets used by the generated client, do not modify the signature.
#pragma warning disable IDE0060
    public SdkException(string message, int statusCode, string responseData_ = null, IReadOnlyDictionary<string, IEnumerable<string>> headers = null, Exception exception = null)
      : base(message, exception)
    {
        ExceptionMessage = message;
        StatusCode = statusCode;
        ResponseData = responseData_;
        Headers = headers;
    }
#pragma warning restore IDE0060
}

public class SdkException<TResult> : SdkException
{
    public TResult Result { get; private set; }

    // The signature of this method is what gets used by the generated client, do not modify the signature.
    public SdkException(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, TResult result, Exception innerException)
        : base(message, statusCode, response, headers, innerException)
    {
        Result = result;
    }
}
