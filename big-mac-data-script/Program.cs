using assignment_wt2_oauth;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

// https://stackoverflow.com/questions/39573571/net-core-console-application-how-to-configure-appsettings-per-environment
var builder = new ConfigurationBuilder()
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile($"appsettings.json", true, true);

var configuration = builder.Build();

var url = configuration["ElasticSearch:URL"];
var username = configuration["ElasticSearch:Username"];
var password = configuration["ElasticSearch:Password"];

var connectionSettings = new ConnectionSettings(new Uri(url));
connectionSettings.DisableDirectStreaming();
connectionSettings.BasicAuthentication(username, password);
connectionSettings.ServerCertificateValidationCallback(CertificateValidations.AllowAll);

var elasticClient = new ElasticClient(connectionSettings);

var serviceProvider = new ServiceCollection()
  .AddSingleton<IBigMacScraper, BigMacScraper>()
  .AddSingleton<AddToElastic>()
  .AddSingleton<IElasticClient>(elasticClient)
  .BuildServiceProvider();

var bigMacScraper = serviceProvider.GetService<IBigMacScraper>();
var addToElastic = serviceProvider.GetService<AddToElastic>();

var data = await bigMacScraper.GetData();
await addToElastic.AddData(data);
