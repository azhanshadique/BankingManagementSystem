﻿using BankingManagementSystem.Models.ConstraintTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankingManagementSystem.Models
{
	public class User
	{
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRoles Role { get; set; }
    }
}