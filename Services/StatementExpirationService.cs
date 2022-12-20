using Newtonsoft.Json;
using Transit.Models;

namespace Transit.Services;

public class StatementExpirationService : BackgroundService
{
    private IConfiguration _configuration;
    public StatementExpirationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken )
    {
        EndPoint endPoint = new EndPoint();
        endPoint.Address = "my address";
        endPoint.Name = "my name";
        EndPointMapOptions endPointMapOptions = new EndPointMapOptions();
        endPointMapOptions.EndPoints = new List<EndPoint>();
        endPointMapOptions.EndPoints.Add(endPoint);

        var json = JsonConvert.SerializeObject(endPointMapOptions);
        Console.WriteLine(json);
        
        var option = new EndPointMapOptions();
        _configuration.GetSection(EndPointMapOptions.SectionName).Bind(option);
        Console.WriteLine($"Json from config = {JsonConvert.SerializeObject(option)}");
        
        
        
        while (!stoppingToken.IsCancellationRequested)
        {
            
            Console.WriteLine($"Statement expiration service is working...");
            await Task.Delay(1000, stoppingToken);
        }
    }
}