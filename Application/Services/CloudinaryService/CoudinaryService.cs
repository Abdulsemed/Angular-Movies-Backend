using Application.Responses;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Application.Services.CloudinaryService;
public class CloudinaryService : ICloudinaryService
{
    private CloudinarySetting _cloudinarySettings { get; }

    public CloudinaryService(IOptions<CloudinarySetting> cloudinarySettings)
    {
        _cloudinarySettings = cloudinarySettings.Value;
    }

    public async Task<CloudResponse> UploadImageAsync(IFormFile imageFile)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf", ".docx", ".doc", ".xlsx", ".mp4", ".wmv", ".mkv", ".avi", ".mp3", ".ogg", ".ppt", ".pptx" };
        var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
        CloudResponse response;

        if (!allowedExtensions.Contains(extension))
        {
            response = new CloudResponse
            {
                Success = false,
                Message = "Unsupported file type",
            };
        }
        else
        {
            var client = new Cloudinary(new Account(
            _cloudinarySettings.CloudName = Environment.GetEnvironmentVariable("Cloud_Name"),
            _cloudinarySettings.ApiKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY"),
            _cloudinarySettings.ApiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")
        ));

            var uploadParams = new AutoUploadParams()
            {
                File = new FileDescription(imageFile.FileName, imageFile.OpenReadStream()),
                //Transformation = new Transformation().Height(500).Width(500).Crop("fill")
            };
            var uploadResult = await client.UploadAsync(uploadParams);

            var link = await Task.FromResult(uploadResult.SecureUrl.AbsoluteUri);
            response = new CloudResponse
            {
                Link = link,
                Success = true,
                Message = "File uploaded succesfully"
            };
        }
        return response;
    }
    public async Task<CloudResponse> DeleteFile(string publicId)
    {
        var client = new Cloudinary(new Account(
            _cloudinarySettings.CloudName = Environment.GetEnvironmentVariable("Cloud_Name"),
            _cloudinarySettings.ApiKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY"),
            _cloudinarySettings.ApiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")
        ));

        DeletionParams deletionParams = new DeletionParams(publicId)
        {
        };
        var deleteResult = await client.DestroyAsync(deletionParams);
        CloudResponse response;
        if (deleteResult.Result.ToLower() == "ok")
        {
            response = new CloudResponse
            {
                Success = true,
            };
        }
        else
        {
            response = new CloudResponse
            {
                Success = false,
            };
        }

        return response;
    }
}
