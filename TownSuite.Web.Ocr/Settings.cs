namespace TownSuite.Web.Ocr;

public class Settings
{
    public string TessDataFolder { get; init; }
    public string TessLanguage { get; init; }
    
    public static string GetTempFolder()
    {
        return Path.Combine(Path.GetTempPath(), "townsuite", "ocr");
    }
}