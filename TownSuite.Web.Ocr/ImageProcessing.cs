using System.Text;
using Tesseract;

namespace TownSuite.Web.Ocr;

public class ImageProcessing
{
    private readonly Settings _settings;

    public ImageProcessing(Settings settings)
    {
        _settings = settings;
    }

    public async Task<string> GetText(Stream inputstream, string ocrType = "basic")
    {
        string path = System.IO.Path.Combine(Settings.GetTempFolder(), Guid.NewGuid().ToString());
        await using var fstream = new FileStream(path, FileMode.Create);
        await inputstream.CopyToAsync(fstream);
        await fstream.FlushAsync();
        fstream.Close();
        string output = ProcessImage(path, ocrType);

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

    private string ProcessImage(string imagePath, string ocrType)
    {
        using var engine = new TesseractEngine(_settings.TessDataFolder, _settings.TessLanguage, EngineMode.Default);
        using var img = Pix.LoadFromFile(imagePath);
        using var page = engine.Process(img);

        string ocrText;
        if (string.Equals("hocr", ocrType, StringComparison.InvariantCultureIgnoreCase))
        {
            ocrText = page.GetHOCRText(0);
        }
        else
        {
            ocrText = page.GetText();
        }

        var sb = new StringBuilder();
        StringReader strReader = new StringReader(ocrText);
        while (true)
        {
            string line = strReader.ReadLine();
            if (!string.IsNullOrWhiteSpace(line))
            {
                sb.AppendLine(line);
            }

            if (line == null)
            {
                break;
            }
        }

        return sb.ToString();
    }
}