﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities.DTOs
{
    public class RequestLogin
    {
        public string Login { get; set; }   
        public string Password { get; set; }
    }
}
