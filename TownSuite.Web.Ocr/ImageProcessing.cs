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

    public async Task<string> GetText(Stream inputstream, string ocrType, string uzn)
    {
        string basePath = System.IO.Path.Combine(Settings.GetTempFolder(), Guid.NewGuid().ToString());
        string path = $"{basePath}.image";
        string uznFile = $"{path}.uzn";
        await using var fstream = new FileStream(path, FileMode.Create);
        await inputstream.CopyToAsync(fstream);
        await fstream.FlushAsync();
        fstream.Close();

        if (!string.IsNullOrWhiteSpace(uzn))
        {
            byte[] uznData = Convert.FromBase64String(uzn);
            string uznString = Encoding.UTF8.GetString(uznData);
            await System.IO.File.WriteAllTextAsync(uznFile, uznString);
        }

        string output = ProcessImage(path, ocrType, uznFile);

        try
        {
            System.IO.File.Delete(path);

            if (System.IO.File.Exists(uznFile))
            {
                System.IO.File.Delete($"{path}.uzn");
            }
        }
        catch (Exception)
        {
            Console.Error.WriteLine($"Failed to delete temp file {path}");
        }

        return output;
    }

    private string ProcessImage(string imagePath, string ocrType, string uznPath)
    {
        using var engine = new TesseractEngine(_settings.TessDataFolder, _settings.TessLanguage, EngineMode.Default);
        if (System.IO.File.Exists(uznPath))
        {
            // https://mariogarcia.github.io/blog/2016/11/tesseract.html#:~:text=uzn%20is%20a%20simple%20text,sections%20of%20a%20scanned%20image.
            engine.SetVariable("tessedit_pageseg_mode", "4"); // Set the PageSegmentationMode
        }

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