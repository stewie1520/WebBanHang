﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanHang.Models
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = null;
        public Pagination Pagination { get; set; } = null;
        public ErrorCode Code { get; set; } = ErrorCode.NONE; // indicate that no error
    }
}
