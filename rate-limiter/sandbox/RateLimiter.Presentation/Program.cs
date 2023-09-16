namespace RateLimiter.Presentation;

public static class Program
{
	public static async Task Main(string[] args)
	{
		var host = CreateWebHostBuilder(args).Build();
		await host.StartAsync();
	}

	private static IHostBuilder CreateWebHostBuilder(string[] args)
	{
		return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
		{
			webBuilder.UseStartup<Startup>();
		});
	}
}