using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection();

serviceProvider.AddSingleton<IBigMacScraper, BigMacScraper>();
