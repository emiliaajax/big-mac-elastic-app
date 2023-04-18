using assignment_wt2_oauth;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
  .AddSingleton<IBigMacScraper, BigMacScraper>()
  .BuildServiceProvider();

var bigMacScraper = serviceProvider.GetService<IBigMacScraper>();

var data = await bigMacScraper.GetData();
