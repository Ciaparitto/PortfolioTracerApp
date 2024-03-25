namespace PortfolioApp.Services.Interfaces
{
	public interface ISecretsGetter
	{
		public string GetSecret(string secretName);
	}
}
