using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Tesseract;

namespace TownSuite.Web.Ocr.Controllers;

[ApiController]
[Route("[controller]")]
public class OcrController : ControllerBase
{
    private readonly ILogger<OcrController> _logger;
    private readonly Settings _settings;

    public OcrController(ILogger<OcrController> logger, Settings settings)
    {
        _logger = logger;
        _settings = settings;
    }

    [Authorize(Policy = "ValidAccessToken")]
    [HttpPost()]
    public async Task<string> Post()
    {
        StringValues ocrType = "basic";
        Request.Headers.TryGetValue("uzn-file", out var uzn);
        Request.Headers.TryGetValue("ocr-type", out ocrType);
        
        var processor = new ImageProcessing(_settings);
        return await processor.GetText(Request.Body, ocrType, uzn);
    }
}