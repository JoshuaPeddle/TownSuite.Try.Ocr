using System.IO;
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
        string tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        string uznFilePath = $"{tempFilePath}.uzn";
        bool createdUZN = false;
        try
        {
            using (var fstream = new FileStream(tempFilePath, FileMode.Create))
            {
                await inputstream.CopyToAsync(fstream);
                await fstream.FlushAsync();
            }

            if (!string.IsNullOrWhiteSpace(uzn))
            {
                byte[] uznData = Convert.FromBase64String(uzn);
                string uznString = Encoding.UTF8.GetString(uznData);
                await File.WriteAllTextAsync(uznFilePath, uznString);
                createdUZN = true;
            }

            return ProcessImage(tempFilePath, ocrType, uznFilePath);
        }
        finally
        {
            File.Delete(tempFilePath);
            if (createdUZN)
                File.Delete(uznFilePath);
        }
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

        using var preProcessedImage = ImagePreprocessor.Preprocess(img);

        using var page = engine.Process(preProcessedImage);

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