using BankingManagementSystem.DAL;
using BankingManagementSystem.Models;
using BankingManagementSystem.Models.API;

namespace BankingManagementSystem.BLL
{
	public class ClientBLL
	{
        public User ValidateClientLogin(AuthRequestDTO client)
        {
            return new ClientDAL().CheckClientCredentials(client);

        }
        public static bool DoesClientExists(string aadhaar, string pan)
        {
            return new ClientDAL().CheckIfClientExists(aadhaar, pan);

        }

    }
}