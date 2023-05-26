using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CompanyEmployees.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : Controller
{
    private readonly IOptionsSnapshot<Configuration.Configuration> _options;
    public TestController(IOptionsSnapshot<Configuration.Configuration> options)
    {
        _options = options;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        return Ok(_options.Value.Message);
    }
}