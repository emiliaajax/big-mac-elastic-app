using assignment_wt2_oauth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// https://stackoverflow.com/questions/39573571/net-core-console-application-how-to-configure-appsettings-per-environment
var builder = new ConfigurationBuilder()
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile($"appsettings.json", true, true);

var configuration = builder.Build();

var serviceProvider = new ServiceCollection()
  .AddSingleton<IBigMacScraper, BigMacScraper>()
  .AddSingleton<AddToElastic>()
  .BuildServiceProvider();

var bigMacScraper = serviceProvider.GetService<IBigMacScraper>();
var addToElastic = serviceProvider.GetService<AddToElastic>();

var data = await bigMacScraper.GetData();
