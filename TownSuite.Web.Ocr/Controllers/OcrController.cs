using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        string path = System.IO.Path.Combine(Settings.GetTempFolder(), Guid.NewGuid().ToString());
        await using var fstream = new FileStream(path, FileMode.Create);
        await Request.Body.CopyToAsync(fstream);
        await fstream.FlushAsync();
        string output = ProcessImage(path);

        try
        {
            System.IO.File.Delete(path);
        }
        catch (Exception)
        {
            Console.Error.WriteLine($"Failed to delete temp file {path}");
        }
        return output;
    }

    private string ProcessImage(string imagePath)
    {
        using var engine = new TesseractEngine(_settings.TessDataFolder, _settings.TessLanguage, EngineMode.Default);
        using var img = Pix.LoadFromFile(imagePath);
        using var page = engine.Process(img);
        var ocrText = page.GetText();

        return ocrText;
    }
}