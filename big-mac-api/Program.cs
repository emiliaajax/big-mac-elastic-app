using Elasticsearch.Net;
using Nest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// https://stackoverflow.com/questions/39573571/net-core-console-application-how-to-configure-appsettings-per-environment
var configBuilder = new ConfigurationBuilder()
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile($"appsettings.json", true, true);

var configuration = configBuilder.Build();

var url = configuration["ElasticSearch:URL"];
var username = configuration["ElasticSearch:Username"];
var password = configuration["ElasticSearch:Password"];

var connectionSettings = new ConnectionSettings(new Uri(url));
connectionSettings.DisableDirectStreaming();
connectionSettings.BasicAuthentication(username, password);
connectionSettings.ServerCertificateValidationCallback(CertificateValidations.AllowAll);

var elasticClient = new ElasticClient(connectionSettings);

var serviceProvider = new ServiceCollection()
  .AddSingleton<IElasticClient>(elasticClient)
  .BuildServiceProvider();

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
