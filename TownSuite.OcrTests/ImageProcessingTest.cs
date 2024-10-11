using TownSuite.Web.Ocr;

namespace TownSuite.OcrTests;

public class Tests
{
    private Settings _settings;

    [SetUp]
    public void Setup()
    {
        _settings = new Settings()
        {
            TessLanguage = "eng",
            TessDataFolder = "./tessdata"
        };
    }

    [TestCase("test.jpg", "test.txt")]
    [TestCase("crankshaft.jpg", "crankshaft.txt")]
    [TestCase("abbott2.jpg", "abbott2.txt")]
    public async Task ParameterizedOcrTest(string imageFileName, string expectedTextFileName)
    {
        var imagePath = Path.Combine("test-images", imageFileName);
        var expectedTextPath = Path.Combine("expected-texts", expectedTextFileName);

        var expected = await File.ReadAllTextAsync(expectedTextPath);
        await using var fs = new FileStream(imagePath, FileMode.Open);
        var processor = new ImageProcessing(_settings);
        var result = await processor.GetText(fs, ocrType: "basic", uzn: null);

        Assert.That(result.Trim(), Is.EqualTo(expected.Trim()));
    }

    [Test]
    public async Task HocrTest()
    {
        const string expected = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"" ""http://www.w3.org/TR/html4/loose.dtd"">
<html>
<head>
<title></title>
<meta http-equiv=""Content-Type"" content=""text/html;charset=utf-8"" />
<meta name='ocr-system' content='tesseract'/>
</head>
<body>
  <div class='ocr_page' id='page_1' title='image ""unknown""; bbox 0 0 227 73; ppageno 0; scan_res 96 96'>
   <div class='ocr_carea' id='block_1_1' title=""bbox 27 38 99 48"">
    <p class='ocr_par' id='par_1_1' lang='eng' title=""bbox 27 38 99 48"">
     <span class='ocr_line' id='line_1_1' title=""bbox 27 38 99 48; baseline 0 0; x_size 20; x_descenders 5; x_ascenders 5"">
      <span class='ocrx_word' id='word_1_1' title='bbox 27 29 59 57; x_wconf 68'>Hello</span>
      <span class='ocrx_word' id='word_1_2' title='bbox 65 29 99 57; x_wconf 68'>World</span>
     </span>
    </p>
   </div>
  </div>
</body>
</html>
";
        await using var fs = new FileStream("test-images/test.jpg", FileMode.Open);
        var processor = new ImageProcessing(_settings);
        var result = await processor.GetText(fs, ocrType: "hocr", uzn: null);

        Assert.That(result, Is.EqualTo(expected));
    }
}