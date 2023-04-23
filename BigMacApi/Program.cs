using BigMacApi.Services;
using Elasticsearch.Net;
using Nest;

// Creating a new WebApplication instance
var builder = WebApplication.CreateBuilder(args);

// Extracting URL, username, and password for ElasticSearch from configuration
var url = builder.Configuration["ElasticSearch:URL"];
var username = builder.Configuration["ElasticSearch:Username"];
var password = builder.Configuration["ElasticSearch:Password"];

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

// Adding required services to the service container
builder.Services
  .AddSingleton<IElasticClient>(elasticClient)
  .AddScoped<IPricesService, PricesService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
