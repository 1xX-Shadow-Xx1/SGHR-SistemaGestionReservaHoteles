﻿

namespace SGHR.Domain.Base
{
    public class OperationResult<T>
    {
        public bool Success { get; set; } = true;
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static OperationResult<T> Ok(T data, string? message = null) => new OperationResult<T> { Success = true, Data = data, Message = message };

        public static OperationResult<T> Fail(string? message) => new OperationResult<T> { Success = false, Message = message };
    }

}
