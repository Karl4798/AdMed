﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdMedWeb.Models
{
    public class User
    {
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
        [Required] [Compare("Password")] public string ConfirmPassword { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }

    }
}