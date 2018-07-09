using System;
using System.Collections.Generic;
using System.Text;

namespace GeohashCross.Models
{
    public class Response<T>
    {
        public Response(T data, bool success, string message)
        {
            Data = data;
            Success = success;
            Message = message;
        }

        public T Data {  get; private set; }
        public string Message { get; private set; } = "";
        public bool Success { get; private set; }
    }
}
