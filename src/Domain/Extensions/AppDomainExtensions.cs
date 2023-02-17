using EasMe.Extensions;
using EasMe.Logging;

namespace Domain.Extensions
{
	public static class AppDomainExtensions
	{
		public static void AddUnexpectedExceptionHandling(this AppDomain domain)
		{
			domain.UnhandledException += (object sender, UnhandledExceptionEventArgs e) =>
			{
				try
				{
					EasLogFactory.StaticLogger.Exception((Exception?)e.ExceptionObject ?? new Exception("FATAL|EXCEPTION IS NULL"), "UnhandledException");
				}
				catch (Exception)
				{
					EasLogFactory.StaticLogger.Fatal(e.ToJsonString(), "UnhandledException");
				}
			};
		}
	}
}
