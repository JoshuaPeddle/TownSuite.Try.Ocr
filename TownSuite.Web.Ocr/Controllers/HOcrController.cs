using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tesseract;

namespace TownSuite.Web.Ocr.Controllers;

[ApiController]
[Route("[controller]")]
public class HOcrController : ControllerBase
{
    private readonly ILogger<HOcrController> _logger;
    private readonly Settings _settings;

    public HOcrController(ILogger<HOcrController> logger, Settings settings)
    {
        _logger = logger;
        _settings = settings;
    }

    [Authorize(Policy = "ValidAccessToken")]
    [HttpPost()]
    public async Task<string> Post()
    {
        var processor = new ImageProcessing(_settings);
        return await processor.GetText(Request.Body, "hocr");
    }
}