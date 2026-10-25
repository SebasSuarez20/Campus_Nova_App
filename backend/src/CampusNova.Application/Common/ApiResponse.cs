using CampusNova.Application.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusNova.Application.Common
{
    public class ApiResponse<T>
    {
        public ResponseStatus Status { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public static ApiResponse<T> Success(T data, string message)
        => new() { Status = ResponseStatus.Success, Message = message, Data = data };

        public static ApiResponse<T> Fail(string message)
            => new() { Status = ResponseStatus.Failed, Message = message, Data = default };
    }
}
