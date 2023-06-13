namespace TownSuite.Web.Ocr;

public class JwtSettings
{
    public string ValidAudience { get; init; }
    public string ValidIssuer { get; init; }
    public string Secret { get; init; }
    public int ValidInMinutes { get; init; }
    public int RefreshTokenValidInMinutes { get; init; }
    public string PolicyName { get; init;}
}