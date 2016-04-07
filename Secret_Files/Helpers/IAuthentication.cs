using System;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace Secret_Files
{
	public interface IAuthentication
	{
		Task<MobileServiceUser> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider);
	}
}

