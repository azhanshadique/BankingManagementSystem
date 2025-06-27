using BankingManagementSystem.DAL;
using BankingManagementSystem.Models.API;
using BankingManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankingManagementSystem.BLL
{
	public class AdminBLL
	{
        public User ValidateAdminLogin(AuthRequestDTO admin)
        {
            return new AdminDAL().CheckAdminCredentials(admin);

        }
    }
}