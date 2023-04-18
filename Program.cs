using assignment_wt2_oauth;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection();

serviceProvider.AddSingleton<IBigMacScraper, BigMacScraper>();
