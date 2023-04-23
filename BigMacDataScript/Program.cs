using BigMacDataScript;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

// Configuration setup using appsettings.json file
// Source: https://stackoverflow.com/questions/39573571/net-core-console-application-how-to-configure-appsettings-per-environment (Retrieved 20/4-23)
var builder = new ConfigurationBuilder()
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile($"appsettings.json", true, true);

var configuration = builder.Build();

// Extracting URL, username, and password for ElasticSearch from configuration
var url = configuration["ElasticSearch:URL"];
var username = configuration["ElasticSearch:Username"];
var password = configuration["ElasticSearch:Password"];

// Checking if the URL configuration is present, if not, throwing an exception
if (url is null)
{
    throw new ArgumentNullException(url, "The ElasticSearch URL configuration is missing.");
}

// Setting up the ElasticSearch client with the extracted URL, username, and password
var connectionSettings = new ConnectionSettings(new Uri(url));
connectionSettings.DisableDirectStreaming();
connectionSettings.BasicAuthentication(username, password);
connectionSettings.ServerCertificateValidationCallback(CertificateValidations.AllowAll);

var elasticClient = new ElasticClient(connectionSettings);

// Setting up the service provider and registering the required services
var serviceProvider = new ServiceCollection()
  .AddSingleton<IBigMacScraper, BigMacScraper>()
  .AddSingleton<AddToElastic>()
  .AddSingleton<IElasticClient>(elasticClient)
  .BuildServiceProvider();

var bigMacScraper = serviceProvider.GetService<IBigMacScraper>();
var addToElastic = serviceProvider.GetService<AddToElastic>();

// Checking if the required services are registered, if not, throwing exceptions
if (bigMacScraper is null)
{
  var errorMessage = "The IBigMacScraper service is not registered.";
  throw new ArgumentNullException(errorMessage);
}

if (addToElastic is null)
{
  var errorMessage = "The AddToElastic service is not registered.";
  throw new ArgumentNullException(errorMessage);
}

// Getting data from the BigMacScraper and adding it to ElasticSearch using AddToElastic service
var data = await bigMacScraper.GetData();
await addToElastic.AddData(data);
