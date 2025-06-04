namespace Application.Services.CloudinaryService;

public class CloudinarySetting
{
    public const string SectionName = "Cloudinary settings";
    public string? CloudName { get; set; }
    public string? ApiKey { get; set; }
    public string? ApiSecret { get; set; }
}
