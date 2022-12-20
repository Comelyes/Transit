using Microsoft.AspNetCore.Mvc;

namespace Transit.Controllers;

public class TestController : Controller
{
    //TODO: Delete...
    [HttpPost]
    [Route("/api/test")]
    public async Task<string> Post(string data = "default")
    {
        return $"{data}";
    }
    [HttpGet]
    [Route("/api/test")]
    public async Task<string> Get(string data = "default")
    {
        return $"{data}";
    }
    [HttpPut]
    [Route("/api/test")]
    public async Task<string> Put(string data = "default")
    {
        return $"{data}";
    }
    [HttpPost]
    [Route("/api/testjson")]
    public async Task<string> Json(string json)
    {
        Console.WriteLine(json);
        return json;
    }
}