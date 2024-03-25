using PortfolioApp.Services.Interfaces;

namespace PortfolioApp.Services
{
	public class SecretGetter : ISecretsGetter
	{
		private readonly IConfiguration _Configuration;
		public SecretGetter(IConfiguration Configuration)
		{
			_Configuration = Configuration;
		}
		public string GetSecret(string secretName)
		{
			var Secret = _Configuration.GetValue<string>(secretName);
			return Secret;
		}
	}
}
