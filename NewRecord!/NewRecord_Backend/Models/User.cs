﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewRecord_Backend.Models
{
    public class User : Person
    {
        public string PasswordHash { get; set; }
    }
}
