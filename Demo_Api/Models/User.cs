﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo_Api.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}