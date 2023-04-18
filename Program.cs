using assignment_wt2_oauth;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
  .AddSingleton<IBigMacScraper, BigMacScraper>()
  .AddSingleton<AddToElastic>()
  .BuildServiceProvider();

var bigMacScraper = serviceProvider.GetService<IBigMacScraper>();
var addToElastic = serviceProvider.GetService<AddToElastic>();

var data = await bigMacScraper.GetData();
