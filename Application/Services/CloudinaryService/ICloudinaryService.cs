using Application.Responses;
using Microsoft.AspNetCore.Http;

namespace Application.Services.CloudinaryService;
public interface ICloudinaryService
{
    Task<CloudResponse> UploadImageAsync(IFormFile imageFile);
    Task<CloudResponse> DeleteFile(string publicId);
}
