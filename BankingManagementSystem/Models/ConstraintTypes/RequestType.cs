using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankingManagementSystem.Models.ConstraintTypes
{
	public enum RequestType
	{
		CreateNewRegistration,
		CreateNewAccount,
		UpdateProfileDetails,
        JointAccountApproval
    }
}